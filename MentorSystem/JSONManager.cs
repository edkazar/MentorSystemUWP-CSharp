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
        public int? annotation_code;
        public List<double> annotation_information;
        public int? selected_annotation_id;
    };

    class JSONManager
    {
        
        private const string CREATE_ANNOTATION_COMMAND = "CreateAnnotationCommand";
        private const string UPDATE_ANNOTATION_COMMAND = "UpdateAnnotationCommand";
        private const string DELETE_ANNOTATION_COMMAND = "DeleteAnnotationCommand";
        private const string POLYLINE_ANNOTATION = "polyline";
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

                        if (!CREATE_ANNOTATION_COMMAND.Equals(to_create.command, StringComparison.Ordinal))
                        {
                            if (to_create.annotation_code == null)
                            {
                                constructLineJSONMessage(to_create.id, to_create.command, to_create.myPoints);/////
                            }
                            else
                            {
                                //constructVirtualAnnotationJSONMessage(to_create.id, to_create.command, to_create.annotation_code, to_create.annotation_information);
                            }
                        }
                        else if (!UPDATE_ANNOTATION_COMMAND.Equals(to_create.command, StringComparison.Ordinal))
                        {
                            if (to_create.annotation_code == null)
                            {
                                constructLineJSONMessage(to_create.id, to_create.command, to_create.myPoints);
                            }
                            else
                            {
                                //constructVirtualAnnotationJSONMessage(to_create.id, to_create.command, to_create.annotation_code, to_create.annotation_information);
                            }
                        }
                        else if (!DELETE_ANNOTATION_COMMAND.Equals(to_create.command, StringComparison.Ordinal))
                        {
                            //constructDeleteJSONMessage(to_create.command, to_create.selected_annotation_id);
                        }
                        JSONs_to_create.Dequeue();
                    }
                }
            }
        }

        public void createJSONable(int id, string command, List<double> myPoints, int? annotation_code,
        List<double> annotation_information, int? selected_annotation_id)
        {
            JSONable to_add = new JSONable();

            to_add.id = id;
            to_add.command = command;
            to_add.myPoints = myPoints;
            to_add.annotation_code = annotation_code;


            to_add.annotation_information = annotation_information;
            to_add.selected_annotation_id = selected_annotation_id;

            JSONs_to_create.Enqueue(to_add);

        }

        private void constructLineJSONMessage(int id, string command, List<double> myPoints)
        {            
            JsonObject message = new JsonObject();
            JsonObject annotation_memory = new JsonObject();
            JsonObject initialAnnotation = new JsonObject();
            JsonArray annotationPoints = new JsonArray();

            //Json::Value message;
            //Json::Value annotation_memory;
            //Json::Value initialAnnotation;
            //Json::Value annotationPoints;

            message.SetNamedValue("id", JsonValue.CreateNumberValue(id));
            message.SetNamedValue("command", JsonValue.CreateStringValue(command));
            //message["id"] = id;
            //message["command"] = command;

            //annotation_memory["matches"] = Json::objectValue;//
            //annotation_memory["initialKeyPoints"] = Json::arrayValue;//
            //annotation_memory["initialDescriptors"] = Json::objectValue;//

            int counter;
            
            for (counter = 0; counter < myPoints.Count(); counter = counter + 2)
            {
                JsonObject newPointAnnotation = new JsonObject();
                newPointAnnotation.SetNamedValue("x", JsonValue.CreateNumberValue(myPoints.ElementAt(counter) / RESOLUTION_X));
                newPointAnnotation.SetNamedValue("y", JsonValue.CreateNumberValue((RESOLUTION_Y - (myPoints.ElementAt(counter + 1))) / RESOLUTION_Y));//check this later on
                annotationPoints.Add(newPointAnnotation);
                /*if (!initialAnnotation.ContainsKey("annotationPoints"))
                {
                    initialAnnotation.SetNamedValue("annotationPoints", annotationPoints);
                }
                else
                {
                    initialAnnotation.Add("annotationPoints", annotationPoints);
                }*/
                

                //annotationPoints["x"] = (double)(myPoints.ElementAt(counter) / RESOLUTION_X);
                //annotationPoints["y"] = (double)((RESOLUTION_Y - (myPoints.ElementAt(counter + 1))) / RESOLUTION_Y);//check this later on

                //initialAnnotation["annotationPoints"].append(annotationPoints);
            }
            initialAnnotation.Add("annotationPoints", annotationPoints);

            initialAnnotation.SetNamedValue("annotationType", JsonValue.CreateStringValue(POLYLINE_ANNOTATION));
            //initialAnnotation["annotationType"] = POLYLINE_ANNOTATION;

            annotation_memory.SetNamedValue("annotation", initialAnnotation);
            //annotation_memory["annotation"] = initialAnnotation;

            /*annotation_memory["initialAnnotation"] = initialAnnotation;//delete
            annotation_memory["currentAnnotation"] = initialAnnotation;//delete
            annotation_memory["currentHomography"] = Json::objectValue;//
            annotation_memory["initialRawKeyPoints"] = Json::arrayValue;//
            annotation_memory["currentRawKeyPoints"] = Json::arrayValue;*///

            message.SetNamedValue("annotation_memory", annotation_memory);
            //message["annotation_memory"] = annotation_memory;

            //Writes JSON Value to a file
            writeJSONonFile(message);
        }

        private async void writeJSONonFile(JsonObject to_text)
        {
            // Create sample file; replace if exists.
            StorageFolder rootFolder = ApplicationData.Current.LocalFolder;
            //StorageFolder projectFolder = await rootFolder.CreateFolderAsync(projectFolderName, CreationCollisionOption.ReplaceExisting);
            //Debug.WriteLine("Location {0}: ", projectFolder.ToString());
            StorageFile sampleFile = await rootFolder.CreateFileAsync("json.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, to_text.Stringify());
        }
    }
}
