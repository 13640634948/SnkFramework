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
using System.Collections.Generic;
using System.Xml.Linq;

namespace SnkFramework.NuGet.Logging
{
    public class SnkLogFactory : ISnkLogFactory
    {
        private Dictionary<string, ISnkLog> repositories = new Dictionary<string, ISnkLog>();
        private SnkLevel _level = SnkLevel.ALL;

        protected virtual Func<string, SnkLogFactory, ISnkLog> loggerCreater => (name, factory) => new SnkConsoleLog(name, factory);

        public SnkLogFactory()
        {
        }

        public SnkLevel Level
        {
            get { return this._level; }
            set { this._level = value; }
        }

        public ISnkLog GetLogger<T>()
        {
            return GetLogger(typeof(T));
        }

        public ISnkLog GetLogger(Type type)
        {
            ISnkLog log;
            if (repositories.TryGetValue(type.FullName, out log))
                return log;

            log = loggerCreater(type.Name, this);
            repositories[type.FullName] = log;
            return log;
        }

        public ISnkLog GetLogger(string name)
        {
            ISnkLog log;
            if (repositories.TryGetValue(name, out log))
                return log;

            log = loggerCreater(name, this);
            repositories[name] = log;
            return log;
        }
    }
}
