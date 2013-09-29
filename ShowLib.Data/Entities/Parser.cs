using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ShowLib.Core;

namespace ShowLib.Data.Entities
{
    public class Parser : BaseObservableModel
    {
        public Parser(ParserType type)
        {
            this._type = type;
        }

        #region Public Methods
        public bool TryParse(string input, out string result)
        {
            bool success = false;

            if (!string.IsNullOrEmpty(this.Pattern))
            {
                string tempResult = null;

                var regex = new Regex(this.Pattern);
                var match = regex.Match(input);

                if (match.Success)
                {
                    tempResult = match.Value;

                    if (tempResult != null && !string.IsNullOrEmpty(this.ExcludedCharacters))
                    {
                        var excludedCharacterArray = this.ExcludedCharacters.ToCharArray();

                        tempResult = new string(tempResult.ToCharArray().Where(c => !excludedCharacterArray.Contains(c)).ToArray());
                    }
                }

                result = tempResult;

                success = tempResult != null;
            }
            else
            {
                success = true;
                result = input;
            }

            return success;
        }
        #endregion

        #region Public Properties
        public ParserType Type
        {
            get { return this._type; }
        }
        
        public string Pattern
        {
            get { return this._pattern; }
            set
            {
                if (this._pattern != value)
                {
                    this._pattern = value;
                    this.RaisePropertyChanged(() => this.Pattern);
                }
            }
        }
        
        public string ExcludedCharacters
        {
            get { return this._excludedCharacters; }
            set
            {
                if (this._excludedCharacters != value)
                {
                    this._excludedCharacters = value;
                    this.RaisePropertyChanged(() => this.ExcludedCharacters);
                }
            }
        }
        #endregion

        #region Private Fields
        private ParserType _type;
        private string _pattern = string.Empty;
        private string _excludedCharacters = string.Empty;
        #endregion
    }
}
