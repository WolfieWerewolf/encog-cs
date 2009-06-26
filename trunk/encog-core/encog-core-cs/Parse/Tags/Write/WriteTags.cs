﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;

namespace Encog.Parse.Tags.Write
{
    /// <summary>
    /// Class used to write out tags, such as XML or HTML.
    /// </summary>
    public class WriteTags
    {

        /// <summary>
        /// The output stream to write to.
        /// </summary>
        private Stream output;

        /// <summary>
        /// Stack to keep track of beginning and ending tags.
        /// </summary>
        private Stack<String> tagStack;

        /// <summary>
        /// The attributes for the current tag.
        /// </summary>
        private IDictionary<String, String> attributes;

        /// <summary>
        /// Used to encode strings to bytes.
        /// </summary>
        private static System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();

        /// <summary>
        /// The logging object.
        /// </summary>
        private readonly ILog logger = LogManager.GetLogger(typeof(WriteTags));


        /// <summary>
        /// Construct an object to write tags.
        /// </summary>
        /// <param name="output">THe output stream.</param>
        public WriteTags(Stream output)
        {
            this.output = output;
            this.tagStack = new Stack<String>();
            this.attributes = new Dictionary<String, String>();
        }

        /// <summary>
        /// Add an attribute to be written with the next tag.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        public void AddAttribute(String name, String value)
        {
            this.attributes.Add(name, value);
        }

        /// <summary>
        /// Add CDATA to the output stream. XML allows a large block of unformatted
        /// text to be added as a CDATA tag.
        /// </summary>
        /// <param name="text">The text to add.</param>
        public void AddCDATA(String text)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('<');
            builder.Append(TagConst.CDATA_BEGIN);
            builder.Append(text);
            builder.Append(TagConst.CDATA_END);
            builder.Append('>');
            try
            {
                byte[] b = encoder.GetBytes(builder.ToString());
                this.output.Write(b, 0, b.Length);
            }
            catch (IOException e)
            {
                throw new ParseError(e);
            }
        }

        /// <summary>
        /// Add a property as a double. A property is a value enclosed in two tags.
        /// </summary>
        /// <param name="name">The name of the enclosing tags.</param>
        /// <param name="d">The value to store.</param>
        public void AddProperty(String name, double d)
        {
            BeginTag(name);
            AddText("" + d);
            EndTag();
        }

        /// <summary>
        /// Add a property as an integer. A property is a value enclosed in two tags.
        /// </summary>
        /// <param name="name">The name of the enclosing tags.</param>
        /// <param name="i">The value to store.</param>
        public void AddProperty(String name, int i)
        {
            AddProperty(name, "" + i);

        }

        /// <summary>
        /// Add a property as a string. A property is a value enclosed in two tags.
        /// </summary>
        /// <param name="name">The name of the enclosing tags.</param>
        /// <param name="str">The value to store.</param>
        public void AddProperty(String name, String str)
        {
            BeginTag(name);
            AddText(str);
            EndTag();
        }

        /// <summary>
        /// Add text.
        /// </summary>
        /// <param name="text">The text to add.</param>
        public void AddText(String text)
        {
            try
            {
                byte[] b = encoder.GetBytes(text);
                this.output.Write(b, 0, b.Length);
            }
            catch (IOException e)
            {
                throw new ParseError(e);
            }
        }

        /// <summary>
        /// Called to begin the document.
        /// </summary>
        public void BeginDocument()
        {
        }

        /// <summary>
        /// Begin a tag with the specified name.
        /// </summary>
        /// <param name="name">The tag to begin.</param>
        public void BeginTag(String name)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<");
            builder.Append(name);
            if (this.attributes.Count > 0)
            {
                foreach (String key in this.attributes.Keys)
                {
                    String value = this.attributes[key];
                    builder.Append(' ');
                    builder.Append(key);
                    builder.Append('=');
                    builder.Append("\"");
                    builder.Append(value);
                    builder.Append("\"");
                }
            }
            builder.Append(">");

            try
            {
                byte[] b = encoder.GetBytes(builder.ToString());
                this.output.Write(b, 0, b.Length);
            }
            catch (IOException e)
            {
                throw new ParseError(e);
            }
            this.attributes.Clear();
            this.tagStack.Push(name);
        }

        /// <summary>
        /// Close this object.
        /// </summary>
        public void Close()
        {
            try
            {
                this.output.Close();
            }
            catch (Exception e)
            {
                throw new EncogError(e);
            }
        }

        /// <summary>
        /// End the document.
        /// </summary>
        public void EndDocument()
        {
        }

        /// <summary>
        /// End the current tag.
        /// </summary>
        public void EndTag()
        {
            if (this.tagStack.Count < 1)
            {
                throw new ParseError(
                        "Can't create end tag, no beginning tag.");
            }
            String tag = this.tagStack.Pop();

            StringBuilder builder = new StringBuilder();
            builder.Append("</");
            builder.Append(tag);
            builder.Append(">");

            try
            {
                byte[] b = encoder.GetBytes(builder.ToString());
                this.output.Write(b, 0, b.Length);
            }
            catch (IOException e)
            {
                throw new ParseError(e);
            }

        }

        /// <summary>
        /// End a tag, require that we are ending the specified tag.
        /// </summary>
        /// <param name="name">The tag to be ending.</param>
        public void EndTag(String name)
        {
            if (!this.tagStack.Peek().Equals(name))
            {
                String str = "End tag mismatch, should be ending: "
                       + this.tagStack.Peek() + ", but trying to end: " + name
                       + ".";
                if (this.logger.IsErrorEnabled)
                {
                    this.logger.Error(str);
                }
                throw new ParseError(str);
            }
            else
            {
                EndTag();
            }

        }
    }

}