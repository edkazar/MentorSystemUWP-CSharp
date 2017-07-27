using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using System.Diagnostics;

namespace MentorSystem
{
    public struct JSONable
    {
        public int id;
        public string command;
        public List<double> myPoints;
        public string annotation_name;
        public List<double> annotation_information;
    };

    class JSONManager
    {
        
        private const string CREATE_ANNOTATION_COMMAND = "CreateAnnotationCommand";
        private const string UPDATE_ANNOTATION_COMMAND = "UpdateAnnotationCommand";
        private const string DELETE_ANNOTATION_COMMAND = "DeleteAnnotationCommand";
        private const string POLYLINE_ANNOTATION = "polyline";
        private const string ICON_ANNOTATION = "tool";
        private const float RESOLUTION_X = 1920.0F;
        private const float RESOLUTION_Y = 1080.0F;

        private Queue<JSONable> JSONs_to_create;
        private bool isJsonBeingCreated;

        public JSONManager()
        {
            JSONs_to_create = new Queue<JSONable>();
            isJsonBeingCreated = false;
        }

        public void constructGeneralJSON()
        {
            while (true)
            {
                if (JSONs_to_create.Count() > 0)
                {
                    if (!isJsonBeingCreated)
                    {
                        isJsonBeingCreated = true;

                        JSONable to_create = JSONs_to_create.First();

                        if (CREATE_ANNOTATION_COMMAND.Equals(to_create.command, StringComparison.Ordinal))
                        {
                            if (to_create.annotation_name == null)
                            {
                                constructLineJSONMessage(to_create.id, to_create.command, to_create.myPoints);
                            }
                            else
                            {
                                constructIconAnnotationJSONMessage(to_create.id, to_create.command, to_create.annotation_name, to_create.annotation_information);
                            }
                        }
                        else if (UPDATE_ANNOTATION_COMMAND.Equals(to_create.command, StringComparison.Ordinal))
                        {
                            if (to_create.annotation_name == null)
                            {
                                constructLineJSONMessage(to_create.id, to_create.command, to_create.myPoints);
                            }
                            else
                            {
                                constructIconAnnotationJSONMessage(to_create.id, to_create.command, to_create.annotation_name, to_create.annotation_information);
                            }
                        }
                        else if (DELETE_ANNOTATION_COMMAND.Equals(to_create.command, StringComparison.Ordinal))
                        {
                            constructDeleteJSONMessage(to_create.id, to_create.command);
                        }
                        JSONs_to_create.Dequeue();
                    }
                }
            }
        }

        public void createJSONable(int id, string command, List<double> myPoints, string annotation_name,
        List<double> annotation_information)
        {
            JSONable to_add = new JSONable();

            to_add.id = id;
            to_add.command = command;
            to_add.myPoints = myPoints;
            to_add.annotation_name = annotation_name;


            to_add.annotation_information = annotation_information;

            JSONs_to_create.Enqueue(to_add);

        }

        private void constructLineJSONMessage(int id, string command, List<double> myPoints)
        {            
            JsonObject message = new JsonObject();
            JsonObject annotation_memory = new JsonObject();
            JsonObject initialAnnotation = new JsonObject();
            JsonArray annotationPoints = new JsonArray();

            message.SetNamedValue("id", JsonValue.CreateNumberValue(id));
            message.SetNamedValue("command", JsonValue.CreateStringValue(command));

            int counter;
            
            for (counter = 0; counter < myPoints.Count(); counter = counter + 2)
            {
                JsonObject newPointAnnotation = new JsonObject();
                newPointAnnotation.SetNamedValue("x", JsonValue.CreateNumberValue(myPoints.ElementAt(counter) / RESOLUTION_X));
                newPointAnnotation.SetNamedValue("y", JsonValue.CreateNumberValue((RESOLUTION_Y - (myPoints.ElementAt(counter + 1))) / RESOLUTION_Y));//check this later on
                annotationPoints.Add(newPointAnnotation);
            }
            initialAnnotation.Add("annotationPoints", annotationPoints);

            initialAnnotation.SetNamedValue("annotationType", JsonValue.CreateStringValue(POLYLINE_ANNOTATION));

            annotation_memory.SetNamedValue("annotation", initialAnnotation);

            message.SetNamedValue("annotation_memory", annotation_memory);

            //Writes JSON Value to a file
            writeJSONonFile(message);
        }

        void constructIconAnnotationJSONMessage(int id, string command, string annotation_name, List<double> annotation_information)
        {
            /*
             * The annotation_information structure contains:
             * annotation_information[0] = annotation center X coordinate
             * annotation_information[1] = annotation center Y coordinate
             * annotation_information[2] = annotation rotation value
             * annotation_information[3] = annotation zoom value
             */

            JsonObject message = new JsonObject();
            JsonObject annotation_memory = new JsonObject();
            JsonObject initialAnnotation = new JsonObject();
            JsonArray annotationPoints = new JsonArray();

            message.SetNamedValue("id", JsonValue.CreateNumberValue(id));
            message.SetNamedValue("command", JsonValue.CreateStringValue(command));

            JsonObject newPointAnnotation = new JsonObject();
            newPointAnnotation.SetNamedValue("x", JsonValue.CreateNumberValue(annotation_information[0] / RESOLUTION_X));
            newPointAnnotation.SetNamedValue("y", JsonValue.CreateNumberValue(annotation_information[1] / RESOLUTION_Y));//check this later on
            annotationPoints.Add(newPointAnnotation);
            initialAnnotation.Add("annotationPoints", annotationPoints);

            initialAnnotation.SetNamedValue("rotation", JsonValue.CreateNumberValue(-1 * (annotation_information[2])));//check
            initialAnnotation.SetNamedValue("scale", JsonValue.CreateNumberValue(annotation_information[3]));
            initialAnnotation.SetNamedValue("annotationType", JsonValue.CreateStringValue(ICON_ANNOTATION));
            initialAnnotation.SetNamedValue("toolType", JsonValue.CreateStringValue(annotation_name));
            initialAnnotation.SetNamedValue("selectableColor", JsonValue.CreateNumberValue(0));

            annotation_memory.SetNamedValue("annotation", initialAnnotation);

            message.SetNamedValue("annotation_memory", annotation_memory);

            //Writes JSON Value to a file
            writeJSONonFile(message);
        }

        void constructDeleteJSONMessage(int id, string command)
        {
            JsonObject message = new JsonObject();

            message.SetNamedValue("id", JsonValue.CreateNumberValue(id));
            message.SetNamedValue("command", JsonValue.CreateStringValue(command));

            //Writes JSON Value to a file
            writeJSONonFile(message);
        }

        private async void writeJSONonFile(JsonObject to_text)
        {
            // Create sample file; replace if exists.
            StorageFolder rootFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await rootFolder.CreateFileAsync("json.txt", CreationCollisionOption.ReplaceExisting);
            string string_to_send = to_text.Stringify();
            await FileIO.WriteTextAsync(sampleFile, string_to_send);

            //Starts the process of sending the value over the network
            JSONtoNetwork(string_to_send);
        }

        /*
         * Method Overview: Routines to send JSON strings over the network
         * Parameters: String containing the JSON Value
         * Return: None
         */
        void JSONtoNetwork(string string_to_send)
        {
            //Makes a copy of the string and transform it into a char*
            string my_string_to_send = string_to_send + "\n";
            char[] message_to_send = my_string_to_send.ToCharArray();

            //Actually sends the message
            //i bnt iResult = myCommunicationManager->sendActionPackets(message_to_send, JSON_NETWORK_CODE);

            //Let the CommanderCenter know that the message was sent
            isJsonBeingCreated = false;
        }
    }
}
