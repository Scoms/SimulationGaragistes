using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class ErrorHandler
    {
        private string _errors;
        private bool _hasErrors;

        public ErrorHandler()
        {
            this._errors = String.Empty;
            this._hasErrors = false;
        }

        public void addError(string error)
        {
            this._errors += error + ". ";
            this._hasErrors = true;
        }

        public bool hasErrors()
        {
            return this._hasErrors;
        }


        public string getErrors()
        {
            return this._errors;
        }
    }
}
