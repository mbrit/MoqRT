using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NETFX_CORE
namespace System
{
	internal sealed class SerializableAttribute : Attribute
	{
	}
}
#endif