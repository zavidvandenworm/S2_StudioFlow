using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Presentation.Mvc.ReusableComponents;


public class ProjectComponent : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);
    }
}