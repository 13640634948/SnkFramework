/*
 * MIT License
 *
 * Copyright (c) 2018 Clark Yang
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in 
 * the Software without restriction, including without limitation the rights to 
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
 * of the Software, and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all 
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
 * SOFTWARE.
 */

using System;

namespace SnkFramework.NuGet.Logging
{
    public class SnkConsoleLog : ISnkLog
    {
        private string name;
        private SnkLogFactory _factory;
        public SnkConsoleLog(string name, SnkLogFactory factory)
        {
            this.name = name;
            this._factory = factory;
        }

        public string Name { get { return this.name; } }

        protected bool IsEnabled(SnkLevel level) => level >= this._factory.Level;

        public virtual bool IsDebugEnabled => IsEnabled(SnkLevel.DEBUG);
        public virtual bool IsInfoEnabled => IsEnabled(SnkLevel.INFO);
        public virtual bool IsWarnEnabled => IsEnabled(SnkLevel.WARN);
        public virtual bool IsErrorEnabled => IsEnabled(SnkLevel.ERROR);
        public virtual bool IsFatalEnabled => IsEnabled(SnkLevel.FATAL);

        protected virtual string Format(object message, string level)
        {
            return string.Format("{0:yyyy-MM-dd HH:mm:ss.fff} [{1}] {2} - {3}", System.DateTime.Now, level, name, message);
        }

        public virtual void Debug(object message)
        {
            Console.WriteLine(Format(message, "DEBUG"));
        }
        public virtual void Info(object message)
        {
            Console.WriteLine(Format(message, "INFO"));
        }
        public virtual void Warn(object message)
        {
            Console.WriteLine(Format(message, "WARN"));
        }
        public virtual void Error(object message)
        {
            Console.WriteLine(Format(message, "ERROR"));
        }

        public virtual void Fatal(object message)
        {
            Console.WriteLine(Format(message, "FATAL"));
        }

        public virtual void Debug(object message, Exception exception) => Debug(string.Format("{0} Exception:{1}", message, exception));
        public virtual void DebugFormat(string format, params object[] args) => Debug(string.Format(format, args));

        public virtual void Info(object message, Exception exception) => Info(string.Format("{0} Exception:{1}", message, exception));
        public virtual void InfoFormat(string format, params object[] args) => Info(string.Format(format, args));

        public virtual void Warn(object message, Exception exception) => Warn(string.Format("{0} Exception:{1}", message, exception));
        public virtual void WarnFormat(string format, params object[] args) => Warn(string.Format(format, args));
       
        public virtual void Error(object message, Exception exception) => Error(string.Format("{0} Exception:{1}", message, exception));
        public virtual void ErrorFormat(string format, params object[] args) => Error(string.Format(format, args));

        public virtual void Fatal(object message, Exception exception) => Fatal(string.Format("{0} Exception:{1}", message, exception));
        public virtual void FatalFormat(string format, params object[] args) => Fatal(string.Format(format, args));

    }
}
