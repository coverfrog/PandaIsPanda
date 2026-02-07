#define RTL_OVERRIDE

using System.Linq;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;

namespace RTLTMPro
{
    [ExecuteInEditMode]
    public class RTLTextMeshPro : TextMeshProUGUI
    {
        // ReSharper disable once InconsistentNaming
#if RTL_OVERRIDE
        public override string text
#else
        public new string text
#endif
        {
            get { return base.text; }
            set
            {
                if (originalText == value)
                    return;

                originalText = value;

                UpdateText();
            }
        }

        public string OriginalText
        {
            get { return originalText; }
        }

        public bool PreserveNumbers
        {
            get { return preserveNumbers; }
            set
            {
                if (preserveNumbers == value)
                    return;

                preserveNumbers = value;
                havePropertiesChanged = true;
            }
        }

        public bool Farsi
        {
            get { return farsi; }
            set
            {
                if (farsi == value)
                    return;

                farsi = value;
                havePropertiesChanged = true;
            }
        }

        public bool FixTags
        {
            get { return fixTags; }
            set
            {
                if (fixTags == value)
                    return;

                fixTags = value;
                havePropertiesChanged = true;
            }
        }

        protected bool ForceFix
        {
            get { return forceFix; }
            set
            {
                if (forceFix == value)
                    return;

                forceFix = value;
                havePropertiesChanged = true;
            }
        }

        [SerializeField]
        protected bool preserveNumbers = true;

        [SerializeField]
        protected bool farsi = false;

        [SerializeField]
        [TextArea(3, 10)]
        protected string originalText;

        [SerializeField]
        protected bool fixTags = true;

        [SerializeField]
        protected bool forceFix = false;

        protected void Update()
        {
            if (havePropertiesChanged)
            {
                UpdateText();
            }
        }

        public void UpdateText()
        {
            if (originalText == null)
                originalText = "";

            if (ForceFix == false && RTLSupport.IsRTLInput(originalText) == false)
            {
                isRightToLeftText = false;
                base.text = originalText;
            }
            else
            {
                isRightToLeftText = true;
                base.text = GetFixedText(originalText);
            }

            havePropertiesChanged = true;
        }

        private System.Collections.Generic.Dictionary<char, char> charDic = new System.Collections.Generic.Dictionary<char, char>()
        {
            { '[', ']' },
            { ']', '[' },
            { '{', '}' },
            { '}', '{' },
        };

        private string ChangeBracket(string text)
        {
            var charArr = text.ToCharArray();
            for(int i = 0; i < charArr.Length; ++i)
                if (charDic.ContainsKey(charArr[i]))
                    charArr[i] = charDic[charArr[i]];
            return new string(charArr);
        }

        private string GetFixedText(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            input = RTLSupport.FixRTL(input, fixTags, preserveNumbers, farsi);
            input = input.Reverse().ToArray().ArrayToString();
            input = ChangeBracket(input);

            return input;
        }
    }
}