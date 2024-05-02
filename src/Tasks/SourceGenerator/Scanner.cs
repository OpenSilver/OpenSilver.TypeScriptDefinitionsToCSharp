
/*===================================================================================
* 
*   Copyright (c) Userware/OpenSilver.net
*      
*   This file is part of the OpenSilver Runtime (https://opensilver.net), which is
*   licensed under the MIT license: https://opensource.org/licenses/MIT
*   
*   As stated in the MIT license, "the above copyright notice and this permission
*   notice shall be included in all copies or substantial portions of the Software."
*  
\*====================================================================================*/

// Generated by TinyPG v1.3 available at www.codeproject.com

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace TinyPG
{
    #region Scanner

    public partial class Scanner
    {
        public string Input;
        public int StartPos = 0;
        public int EndPos = 0;
        public int CurrentLine;
        public int CurrentColumn;
        public int CurrentPosition;
        public List<Token> Skipped; // tokens that were skipped
        public Dictionary<TokenType, Regex> Patterns;

        private Token LookAheadToken;
        private List<TokenType> Tokens;
        private List<TokenType> SkipList; // tokens to be skipped

        Action<string> _methodToUpdateProgress; // NEW

        public Scanner(Action<string> methodToUpdateProgress) // NEW
        {
            _methodToUpdateProgress = methodToUpdateProgress; // NEW

            Regex regex;
            Patterns = new Dictionary<TokenType, Regex>();
            Tokens = new List<TokenType>();
            LookAheadToken = null;
            Skipped = new List<Token>();

            SkipList = new List<TokenType>();
            SkipList.Add(TokenType.WHITESPACE);

            regex = new Regex(@"typeof", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_TYPEOF, regex);
            Tokens.Add(TokenType.K_TYPEOF);

            regex = new Regex(@"class", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_CLASS, regex);
            Tokens.Add(TokenType.K_CLASS);

            regex = new Regex(@"interface", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_INTERFACE, regex);
            Tokens.Add(TokenType.K_INTERFACE);

            regex = new Regex(@"new", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_NEW, regex);
            Tokens.Add(TokenType.K_NEW);

            regex = new Regex(@"function", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_FUNCTION, regex);
            Tokens.Add(TokenType.K_FUNCTION);

            regex = new Regex(@"var", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_VAR, regex);
            Tokens.Add(TokenType.K_VAR);

            regex = new Regex(@"namespace", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_NAMESPACE, regex);
            Tokens.Add(TokenType.K_NAMESPACE);

            regex = new Regex(@"module", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_MODULE, regex);
            Tokens.Add(TokenType.K_MODULE);

            regex = new Regex(@"declare", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_DECLARE, regex);
            Tokens.Add(TokenType.K_DECLARE);

            regex = new Regex(@"extends", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_EXTENDS, regex);
            Tokens.Add(TokenType.K_EXTENDS);

            regex = new Regex(@"implements", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_IMPLEMENTS, regex);
            Tokens.Add(TokenType.K_IMPLEMENTS);

            regex = new Regex(@"static", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_STATIC, regex);
            Tokens.Add(TokenType.K_STATIC);

            regex = new Regex(@"import", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_IMPORT, regex);
            Tokens.Add(TokenType.K_IMPORT);

            regex = new Regex(@"export", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_EXPORT, regex);
            Tokens.Add(TokenType.K_EXPORT);

            regex = new Regex(@"export\s+=", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_EXPORT_EQ, regex);
            Tokens.Add(TokenType.K_EXPORT_EQ);

            regex = new Regex(@"require", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_REQUIRE, regex);
            Tokens.Add(TokenType.K_REQUIRE);

            regex = new Regex(@"get", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_GET, regex);
            Tokens.Add(TokenType.K_GET);

            regex = new Regex(@"set", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_SET, regex);
            Tokens.Add(TokenType.K_SET);

            regex = new Regex(@"readonly", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_READONLY, regex);
            Tokens.Add(TokenType.K_READONLY);

            regex = new Regex(@"enum", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_ENUM, regex);
            Tokens.Add(TokenType.K_ENUM);

            regex = new Regex(@"const", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_CONST, regex);
            Tokens.Add(TokenType.K_CONST);

            regex = new Regex(@"let", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_LET, regex);
            Tokens.Add(TokenType.K_LET);

            regex = new Regex(@"public", RegexOptions.Compiled);
            Patterns.Add(TokenType.K_PUBLIC, regex);
            Tokens.Add(TokenType.K_PUBLIC);

            regex = new Regex(@"[a-zA-Z_$][a-zA-Z0-9_$]*", RegexOptions.Compiled);
            Patterns.Add(TokenType.IDENT, regex);
            Tokens.Add(TokenType.IDENT);

            regex = new Regex(@"\.", RegexOptions.Compiled);
            Patterns.Add(TokenType.DOT, regex);
            Tokens.Add(TokenType.DOT);

            regex = new Regex(@",", RegexOptions.Compiled);
            Patterns.Add(TokenType.COMMA, regex);
            Tokens.Add(TokenType.COMMA);

            regex = new Regex(@":", RegexOptions.Compiled);
            Patterns.Add(TokenType.COLON, regex);
            Tokens.Add(TokenType.COLON);

            regex = new Regex(@";", RegexOptions.Compiled);
            Patterns.Add(TokenType.SEMICOLON, regex);
            Tokens.Add(TokenType.SEMICOLON);

            regex = new Regex(@"\[", RegexOptions.Compiled);
            Patterns.Add(TokenType.LBRACKET, regex);
            Tokens.Add(TokenType.LBRACKET);

            regex = new Regex(@"\]", RegexOptions.Compiled);
            Patterns.Add(TokenType.RBRACKET, regex);
            Tokens.Add(TokenType.RBRACKET);

            regex = new Regex(@"\{", RegexOptions.Compiled);
            Patterns.Add(TokenType.LBRACE, regex);
            Tokens.Add(TokenType.LBRACE);

            regex = new Regex(@"\}", RegexOptions.Compiled);
            Patterns.Add(TokenType.RBRACE, regex);
            Tokens.Add(TokenType.RBRACE);

            regex = new Regex(@"\(", RegexOptions.Compiled);
            Patterns.Add(TokenType.LPAREN, regex);
            Tokens.Add(TokenType.LPAREN);

            regex = new Regex(@"\)", RegexOptions.Compiled);
            Patterns.Add(TokenType.RPAREN, regex);
            Tokens.Add(TokenType.RPAREN);

            regex = new Regex(@"\|", RegexOptions.Compiled);
            Patterns.Add(TokenType.VBAR, regex);
            Tokens.Add(TokenType.VBAR);

            regex = new Regex(@"<", RegexOptions.Compiled);
            Patterns.Add(TokenType.INFERIOR, regex);
            Tokens.Add(TokenType.INFERIOR);

            regex = new Regex(@">", RegexOptions.Compiled);
            Patterns.Add(TokenType.SUPERIOR, regex);
            Tokens.Add(TokenType.SUPERIOR);

            regex = new Regex(@"\?", RegexOptions.Compiled);
            Patterns.Add(TokenType.QUESTION, regex);
            Tokens.Add(TokenType.QUESTION);

            regex = new Regex(@"=>", RegexOptions.Compiled);
            Patterns.Add(TokenType.FATARROW, regex);
            Tokens.Add(TokenType.FATARROW);

            regex = new Regex(@"\.\.\.", RegexOptions.Compiled);
            Patterns.Add(TokenType.SPREADOP, regex);
            Tokens.Add(TokenType.SPREADOP);

            regex = new Regex(@"'", RegexOptions.Compiled);
            Patterns.Add(TokenType.SIMPLEQUOTE, regex);
            Tokens.Add(TokenType.SIMPLEQUOTE);

            regex = new Regex(@"""", RegexOptions.Compiled);
            Patterns.Add(TokenType.DOUBLEQUOTE, regex);
            Tokens.Add(TokenType.DOUBLEQUOTE);

            regex = new Regex(@"=", RegexOptions.Compiled);
            Patterns.Add(TokenType.EQUALS, regex);
            Tokens.Add(TokenType.EQUALS);

            regex = new Regex(@"^$", RegexOptions.Compiled);
            Patterns.Add(TokenType.EOF, regex);
            Tokens.Add(TokenType.EOF);

            regex = new Regex(@"\$", RegexOptions.Compiled);
            Patterns.Add(TokenType.DOLLAR, regex);
            Tokens.Add(TokenType.DOLLAR);

            regex = new Regex(@""".*""", RegexOptions.Compiled);
            Patterns.Add(TokenType.STRING, regex);
            Tokens.Add(TokenType.STRING);

            regex = new Regex(@"[a-zA-Z_][a-zA-Z0-9_]*(\.[a-zA-Z_][a-zA-Z0-9_]*)*", RegexOptions.Compiled);
            Patterns.Add(TokenType.DOTIDENT, regex);
            Tokens.Add(TokenType.DOTIDENT);

            regex = new Regex(@"[a-zA-Z_\-\/][a-zA-Z0-9_\-\/]*(\.[a-zA-Z_\-\/][a-zA-Z0-9_\-\/]*)*", RegexOptions.Compiled);
            Patterns.Add(TokenType.DOTIDENT_WITH_ADDITIONAL_CHARS_ALLOWED, regex);
            Tokens.Add(TokenType.DOTIDENT_WITH_ADDITIONAL_CHARS_ALLOWED);

            regex = new Regex(@"([a-zA-Z_][a-zA-Z0-9_]*)?(<.*>)?\??\s*\(", RegexOptions.Compiled);
            Patterns.Add(TokenType.FUNCTION, regex);
            Tokens.Add(TokenType.FUNCTION);

            regex = new Regex(@"[a-zA-Z_][a-zA-Z0-9_]*(\.[a-zA-Z_][a-zA-Z0-9_]*)*<", RegexOptions.Compiled);
            Patterns.Add(TokenType.GENERIC, regex);
            Tokens.Add(TokenType.GENERIC);

            regex = new Regex(@"\[\]", RegexOptions.Compiled);
            Patterns.Add(TokenType.ARRAYLEVEL, regex);
            Tokens.Add(TokenType.ARRAYLEVEL);

            regex = new Regex(@"<[a-zA-Z0-9_](, [a-zA-Z0-9_])*>", RegexOptions.Compiled);
            Patterns.Add(TokenType.GENERIC_ARG, regex);
            Tokens.Add(TokenType.GENERIC_ARG);

            regex = new Regex(@"\s+", RegexOptions.Compiled);
            Patterns.Add(TokenType.WHITESPACE, regex);
            Tokens.Add(TokenType.WHITESPACE);


        }

        public void Init(string input)
        {
            this.Input = input;
            StartPos = 0;
            EndPos = 0;
            CurrentLine = 0;
            CurrentColumn = 0;
            CurrentPosition = 0;
            LookAheadToken = null;
        }

        public Token GetToken(TokenType type)
        {
            Token t = new Token(this.StartPos, this.EndPos);
            t.Type = type;
            return t;
        }

         /// <summary>
        /// executes a lookahead of the next token
        /// and will advance the scan on the input string
        /// </summary>
        /// <returns></returns>
        public Token Scan(params TokenType[] expectedtokens)
        {
            Token tok = LookAhead(expectedtokens); // temporarely retrieve the lookahead
            LookAheadToken = null; // reset lookahead token, so scanning will continue
            StartPos = tok.EndPos;
            EndPos = tok.EndPos; // set the tokenizer to the new scan position
            return tok;
        }

        /// <summary>
        /// returns token with longest best match
        /// </summary>
        /// <returns></returns>
        public Token LookAhead(params TokenType[] expectedtokens)
        {
            int i;
            int startpos = StartPos;
            Token tok = null;
            List<TokenType> scantokens;


            // this prevents double scanning and matching
            // increased performance
            if (LookAheadToken != null 
                && LookAheadToken.Type != TokenType._UNDETERMINED_ 
                && LookAheadToken.Type != TokenType._NONE_) return LookAheadToken;

            // if no scantokens specified, then scan for all of them (= backward compatible)
            if (expectedtokens.Length == 0)
                scantokens = Tokens;
            else
            {
                scantokens = new List<TokenType>(expectedtokens);
                scantokens.AddRange(SkipList);
            }

            do
            {

                int len = -1;
                TokenType index = (TokenType)int.MaxValue;
                string input = Input.Substring(startpos);

                tok = new Token(startpos, EndPos);

                for (i = 0; i < scantokens.Count; i++)
                {
                    Regex r = Patterns[scantokens[i]];
                    Match m = r.Match(input);
                    if (m.Success && m.Index == 0 && ((m.Length > len) || (scantokens[i] < index && m.Length == len )))
                    {
                        len = m.Length;
                        index = scantokens[i];  
                    }
                }

                if (index >= 0 && len >= 0)
                {
                    tok.EndPos = startpos + len;
                    tok.Text = Input.Substring(tok.StartPos, len);
                    tok.Type = index;
                }
                else if (tok.StartPos < tok.EndPos - 1)
                {
                    tok.Text = Input.Substring(tok.StartPos, 1);
                }

                if (SkipList.Contains(tok.Type))
                {
                    startpos = tok.EndPos;
                    Skipped.Add(tok);
                }
                else
                {
                    // only assign to non-skipped tokens
                    tok.Skipped = Skipped; // assign prior skips to this token
                    Skipped = new List<Token>(); //reset skips
                }
            }
            while (SkipList.Contains(tok.Type));

            LookAheadToken = tok;

            if (tok.Type == TokenType.DOTIDENT)  // NEW
                _methodToUpdateProgress(tok.Text);  // NEW

            return tok;
        }
    }

    #endregion

    #region Token

    public enum TokenType
    {

            //Non terminal tokens:
            _NONE_  = 0,
            _UNDETERMINED_= 1,

            //Non terminal tokens:
            Start   = 2,
            Namespace= 3,
            NamespaceContent= 4,
            Generic = 5,
            FunctionType= 6,
            Typeof  = 7,
            AnonymousType= 8,
            Type    = 9,
            Variable= 10,
            Indexer = 11,
            ParamList= 12,
            Function= 13,
            BlockElement= 14,
            Interface= 15,
            Class   = 16,
            AccessTag= 17,
            Getter  = 18,
            Setter  = 19,
            Extends = 20,
            Export  = 21,
            Import  = 22,
            Enum    = 23,

            //Terminal tokens:
            K_TYPEOF= 24,
            K_CLASS = 25,
            K_INTERFACE= 26,
            K_NEW   = 27,
            K_FUNCTION= 28,
            K_VAR   = 29,
            K_NAMESPACE= 30,
            K_MODULE= 31,
            K_DECLARE= 32,
            K_EXTENDS= 33,
            K_IMPLEMENTS= 34,
            K_STATIC= 35,
            K_IMPORT= 36,
            K_EXPORT= 37,
            K_EXPORT_EQ= 38,
            K_REQUIRE= 39,
            K_GET   = 40,
            K_SET   = 41,
            K_READONLY= 42,
            K_ENUM  = 43,
            K_CONST = 44,
            K_LET   = 45,
            K_PUBLIC= 46,
            IDENT   = 47,
            DOT     = 48,
            COMMA   = 49,
            COLON   = 50,
            SEMICOLON= 51,
            LBRACKET= 52,
            RBRACKET= 53,
            LBRACE  = 54,
            RBRACE  = 55,
            LPAREN  = 56,
            RPAREN  = 57,
            VBAR    = 58,
            INFERIOR= 59,
            SUPERIOR= 60,
            QUESTION= 61,
            FATARROW= 62,
            SPREADOP= 63,
            SIMPLEQUOTE= 64,
            DOUBLEQUOTE= 65,
            EQUALS  = 66,
            EOF     = 67,
            DOLLAR  = 68,
            STRING  = 69,
            DOTIDENT= 70,
            DOTIDENT_WITH_ADDITIONAL_CHARS_ALLOWED= 71,
            FUNCTION= 72,
            GENERIC = 73,
            ARRAYLEVEL= 74,
            GENERIC_ARG= 75,
            WHITESPACE= 76
    }

    public class Token
    {
        private int startpos;
        private int endpos;
        private string text;
        private object value;

        // contains all prior skipped symbols
        private List<Token> skipped;

        public int StartPos { 
            get { return startpos;} 
            set { startpos = value; }
        }

        public int Length { 
            get { return endpos - startpos;} 
        }

        public int EndPos { 
            get { return endpos;} 
            set { endpos = value; }
        }

        public string Text { 
            get { return text;} 
            set { text = value; }
        }

        public List<Token> Skipped { 
            get { return skipped;} 
            set { skipped = value; }
        }
        public object Value { 
            get { return value;} 
            set { this.value = value; }
        }

        [XmlAttribute]
        public TokenType Type;

        public Token()
            : this(0, 0)
        {
        }

        public Token(int start, int end)
        {
            Type = TokenType._UNDETERMINED_;
            startpos = start;
            endpos = end;
            Text = ""; // must initialize with empty string, may cause null reference exceptions otherwise
            Value = null;
        }

        public void UpdateRange(Token token)
        {
            if (token.StartPos < startpos) startpos = token.StartPos;
            if (token.EndPos > endpos) endpos = token.EndPos;
        }

        public override string ToString()
        {
            if (Text != null)
                return Type.ToString() + " '" + Text + "'";
            else
                return Type.ToString();
        }
    }

    #endregion
}