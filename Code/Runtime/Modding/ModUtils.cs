using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.Code.Runtime.Modding
{
	class ModUtils
	{
		public static readonly string QUOTE = "\"";
		public static readonly string SandboxCode = @"
-- Earthian Sandbox Begin.

print(" + QUOTE + @"[API] removing or changing variables in the Earthian api can lead to your mod being unusable, and will be skipped under loading." + QUOTE + @")

local sandBoxRequire = require
-- functions with messages descriping that they are disabled.
os = function() print(" + QUOTE + @"os calls has been disabled." + QUOTE + @") end
io = function() print(" + QUOTE + @"io calls has been disabled." + QUOTE + @") end
panic = function() print(" + QUOTE + @"Computer says no." + QUOTE + @") end
nilall = function() print(" + QUOTE + @"I (the api) don't really feel like setting all these variables to nil." + QUOTE + @") end
nullall = function() print(" + QUOTE + @"I (the api) don't really feel like setting all these variables to null (Even though that is C# code and setting something to null would not work out of the box. lol)" + QUOTE + @") end
import = function() print(" + QUOTE + @"Running of unsafe code has been disabled." + QUOTE + @") end
package = function() print(" + QUOTE + @"Blocked attempt at calling package." + QUOTE + @") end
module = function() print(" + QUOTE + @"Blocked attempt at calling module." + QUOTE + @") end
dofile = function() print(" + QUOTE + @"Running of files from outside the Earthian API environment has been disabled." + QUOTE + @") end
getfenv = function() print(" + QUOTE + @"Blocked attempt at calling getfenv." + QUOTE + @") end
getmetatable = function() print(" + QUOTE + @"Blocked attempt at calling getmetatable." + QUOTE + @") end
rawequal = function() print(" + QUOTE + @"Blocked attempt at calling rawequal." + QUOTE + @") end
rawget = function() print(" + QUOTE + @"Blocked attempt at calling rawget." + QUOTE + @") end
rawset = function() print(" + QUOTE + @"Blocked attempt at calling rawset." + QUOTE + @") end
setfenv = function() print(" + QUOTE + @"Blocked attempt at calling setfenv." + QUOTE + @") end
setmetatable = function() print(" + QUOTE + @"Blocked attempt at calling setmetatable." + QUOTE + @") end





-- Just nil'd functions
load = nil
debug = nil
newproxy = nil
collectgarbage = nil
require = nil
loadfile = nil
loadstring = nil";
		
	}
}
