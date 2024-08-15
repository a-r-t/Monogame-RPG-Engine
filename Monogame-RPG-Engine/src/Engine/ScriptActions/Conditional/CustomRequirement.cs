using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ScriptActions.Conditional
{
    public class CustomRequirement : Requirement
    {
        private Func<bool> isRequirementMetFunction;
        
        public CustomRequirement(Func<bool> isRequirementMetFunction)
        {
            this.isRequirementMetFunction = isRequirementMetFunction;
        }

        public virtual bool IsRequirementMet() { 
            return isRequirementMetFunction?.Invoke() ?? false; 
        }
    }
}
