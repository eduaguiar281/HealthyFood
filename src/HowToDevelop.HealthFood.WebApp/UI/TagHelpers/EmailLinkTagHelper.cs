using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.WebApp.UI.TagHelpers
{
    public class EmailLinkTagHelper: TagHelper
    {
        //PascalCase é traduzido para kebab-case
        //DominioEmail -> dominio-email
        public string DominioEmail { get; set; } = "howtodevelop.com.br";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";

            var content = await output.GetChildContentAsync();
            var target = content.GetContent() + "@" + DominioEmail;

            output.Attributes.SetAttribute("href", "mailto:" + target);

            output.Content.SetContent(target);
        }

    }
}
