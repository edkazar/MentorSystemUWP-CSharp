using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorSystemWebRTC.MentorClasses
{
    class ControlCenter
    {
        //Virtual annotation selected flag
        private bool IconAnnotationSelected;

        //Path to the selected icon annotation
        private string IconAnnotationPath;

        //Class constructor
        public ControlCenter()
        {
            IconAnnotationSelected = false;

            IconAnnotationPath = "";
        }

        //Gets the virtual annotation selected flag
        public bool GetIconAnnotationSelectedFlag()
        {
            return IconAnnotationSelected;
        }

        //Changes the state of the virtual annotation selected flag
        public void SetIconAnnotationSelectedFlag(bool pFlag)
        {
            IconAnnotationSelected = pFlag;
        }

        //Gets the path to the selected icon annotation
        public string GetSelectedIconPath()
        {
            return IconAnnotationPath;
        }

        //Sets the path to the selected icon annotation
        public void SetSelectedIconPath(string pPath)
        {
            IconAnnotationPath = pPath;
        }
    }
}
