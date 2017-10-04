///////////////////////////////////////////////////////////////////
/*
 * Mentor System Application
 * System for Telemementoring with Augmented Reality (STAR) Project
 * Intelligent Systems and Assistive Technology (ISAT) Laboratory
 * Purdue University School of Industrial Engineering
 * 
 * Code programmed by: Edgar Javier Rojas Muñoz
 * advised by the professor: Juan Pablo Wachs, Ph.D
 */
//---------------------------------------------------------------//
/*                        CODE OVERVIEW
 * Name: JSONManager.cs
 *
 * Overview: This class uses the Json communication protocol to 
 * create a series of messages to be sent to a client. Those 
 * messages will contain the required information about the 
 * annotations (either icon or line type) and will be then send to
 * the CommunicationManager for it to send the messages over the
 * network.
 */
///////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Data.Json;
using Windows.Storage;
using Windows.Networking.Sockets;

namespace MentorSystemWebRTC.MentorClasses
{
    // Used to store the information to be sent by the JSON in a 
    // general way
    public struct JSONable
    {
        public int id; // Id of the annotation
        public string command; // Performed command
        public List<double> myPoints; // Points representing line
        public string annotation_name; // Name of the icon annotation
        public List<double> annotation_information; // Rot,scale,etc.
    };

    class JSONManager
    {  
        // Constant definitions used by the class
        private const string CREATE_ANNOTATION_COMMAND = "CreateAnnotationCommand";
        private const string UPDATE_ANNOTATION_COMMAND = "UpdateAnnotationCommand";
        private const string DELETE_ANNOTATION_COMMAND = "DeleteAnnotationCommand";
        private const string POLYLINE_ANNOTATION = "polyline";
        private const string ICON_ANNOTATION = "tool";
        private const float RESOLUTION_X = 1920.0F;
        private const float RESOLUTION_Y = 1080.0F;

        // List of JSON messages to be created
        private Queue<JSONable> JSONs_to_create;

        // Flag about if a JSON is being created right now
        private bool isJsonBeingCreated;

        // Socket to send the JSON info
        private StreamSocket JsonSocket;

        /*
         * Method Overview: Constructor of the class
         * Parameters: None
         * Return: Instance of the class
         */
        public JSONManager()
        {
            JSONs_to_create = new Queue<JSONable>();
            isJsonBeingCreated = false;
        }

        /*
         * Method Overview: Infinite loop to start the JSON creation
         * Parameters: None
         * Return: None
         */
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

        /*
         * Method Overview: Constructs a JSON Value object of a line
         * Parameters: Required values of the object to create
         * Return: None
         */
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

        /*
         * Method Overview: Constructs a JSON Object object of a line
         * Parameters: Line Id, message command, points of the line
         * Return: None
         */
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
                newPointAnnotation.SetNamedValue("y", JsonValue.CreateNumberValue(myPoints.ElementAt(counter + 1) / RESOLUTION_Y));
                annotationPoints.Add(newPointAnnotation);
            }
            initialAnnotation.Add("annotationPoints", annotationPoints);

            initialAnnotation.SetNamedValue("annotationType", JsonValue.CreateStringValue(POLYLINE_ANNOTATION));

            annotation_memory.SetNamedValue("annotation", initialAnnotation);

            message.SetNamedValue("annotation_memory", annotation_memory);

            //Writes JSON Value to a file
            writeJSONonFile(message);
        }

        /*
         * Method Overview: Constructs a JSON Object object of an annotation
         * Parameters (1): Annotation Id, message command, annotation name
         * Parameters (2): Annotation's important information
         * Return: None
         */
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
            newPointAnnotation.SetNamedValue("y", JsonValue.CreateNumberValue(annotation_information[1] / RESOLUTION_Y));
            annotationPoints.Add(newPointAnnotation);
            initialAnnotation.Add("annotationPoints", annotationPoints);

            initialAnnotation.SetNamedValue("rotation", JsonValue.CreateNumberValue(-1 * (annotation_information[2]+45)));
            initialAnnotation.SetNamedValue("scale", JsonValue.CreateNumberValue(annotation_information[3]*0.09f));
            initialAnnotation.SetNamedValue("annotationType", JsonValue.CreateStringValue(ICON_ANNOTATION));
            initialAnnotation.SetNamedValue("toolType", JsonValue.CreateStringValue(annotation_name));
            initialAnnotation.SetNamedValue("selectableColor", JsonValue.CreateNumberValue(0));

            annotation_memory.SetNamedValue("annotation", initialAnnotation);

            message.SetNamedValue("annotation_memory", annotation_memory);

            //Writes JSON Value to a file
            writeJSONonFile(message);
        }

        /*
         * Method Overview: Creates a JSON Message of a delete command
         * Parameters: Command type, ID of the erased annotations
         * Return: None
         */
        void constructDeleteJSONMessage(int id, string command)
        {
            JsonObject message = new JsonObject();

            message.SetNamedValue("id", JsonValue.CreateNumberValue(id));
            message.SetNamedValue("command", JsonValue.CreateStringValue(command));

            //Writes JSON Value to a file
            writeJSONonFile(message);
        }

        /*
         * Method Overview: Writes a JSON Value to a file
         * Parameters: JSON Value to write
         * Return: None
         */
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
        private async void JSONtoNetwork(string string_to_send)
        {
            //Send the line back to the remote client.
            if (JsonSocket != null)
            {
                Stream outStream = JsonSocket.OutputStream.AsStreamForWrite();
                StreamWriter writer = new StreamWriter(outStream);
                await writer.WriteLineAsync(string_to_send);
                await writer.FlushAsync();
            }

            //Let the CommanderCenter know that the message was sent
            isJsonBeingCreated = false;
        }

        /*
         * Method Overview: Assign the socket that is used to communicate through the network
         * Parameters: Socket to use
         * Return: None
         */
        public async void receiveSocket(StreamSocket socket)
        {
            JsonSocket = socket;
        }

        /*
         * Method Overview: Method to convert a string into a streamto send over network
         * Parameters: String to convert
         * Return: None
         */
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
