// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.Core.Logging
{
	using System;

	///<summary>
	///  The Null Logger class.  This is useful for implementations where you need
	///  to provide a logger to a utility class, but do not want any output from it.
	///  It also helps when you have a utility that does not have a logger to supply.
	///</summary>
	public class NullLogger : ILogger
	{
		internal static ILogger Instance = new NullLogger();

		public bool IsDebugEnabled
		{
			get
			{
				return false;
			}
		}

		public bool IsErrorEnabled
		{
			get
			{
				return false;
			}
		}

		public bool IsFatalEnabled
		{
			get
			{
				return false;
			}
		}

		public bool IsInfoEnabled
		{
			get
			{
				return false;
			}
		}

		public bool IsWarnEnabled
		{
			get
			{
				return false;
			}
		}

		public ILogger CreateChildLogger(string loggerName)
		{
			return Instance;
		}

		public void Debug(string message)
		{
		}

		public void Debug(Func<string> messageFactory)
		{
		}

		public void Debug(string message, Exception exception)
		{
		}

		public void DebugFormat(string format, params object[] args)
		{
		}

		public void DebugFormat(Exception exception, string format, params object[] args)
		{
		}

		public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
		}

		public void DebugFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
		{
		}

		public void Error(string message)
		{
		}

		public void Error(Func<string> messageFactory)
		{
		}

		public void Error(string message, Exception exception)
		{
		}

		public void ErrorFormat(string format, params object[] args)
		{
		}

		public void ErrorFormat(Exception exception, string format, params object[] args)
		{
		}

		public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
		}

		public void ErrorFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
		{
		}

		public void Fatal(string message)
		{
		}

		public void Fatal(Func<string> messageFactory)
		{
		}

		public void Fatal(string message, Exception exception)
		{
		}

		public void FatalFormat(string format, params object[] args)
		{
		}

		public void FatalFormat(Exception exception, string format, params object[] args)
		{
		}

		public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
		}

		public void FatalFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
		{
		}

		public void Info(string message)
		{
		}

		public void Info(Func<string> messageFactory)
		{
		}

		public void Info(string message, Exception exception)
		{
		}

		public void InfoFormat(string format, params object[] args)
		{
		}

		public void InfoFormat(Exception exception, string format, params object[] args)
		{
		}

		public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
		}

		public void InfoFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
		{
		}

		public void Warn(string message)
		{
		}

		public void Warn(Func<string> messageFactory)
		{
		}

		public void Warn(string message, Exception exception)
		{
		}

		public void WarnFormat(string format, params object[] args)
		{
		}

		public void WarnFormat(Exception exception, string format, params object[] args)
		{
		}

		public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
		}

		public void WarnFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
		{
		}
	}
}