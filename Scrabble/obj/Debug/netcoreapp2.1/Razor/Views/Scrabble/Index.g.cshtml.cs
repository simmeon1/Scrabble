#pragma checksum "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a6975ac8bbd94cff67c46eba6a7ee5bf47c7e039"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Scrabble_Index), @"mvc.1.0.view", @"/Views/Scrabble/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Scrabble/Index.cshtml", typeof(AspNetCore.Views_Scrabble_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\_ViewImports.cshtml"
using Scrabble;

#line default
#line hidden
#line 2 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\_ViewImports.cshtml"
using Scrabble.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a6975ac8bbd94cff67c46eba6a7ee5bf47c7e039", @"/Views/Scrabble/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"88ff2211d293440a90f2e8e07e96277ac6cf43d6", @"/Views/_ViewImports.cshtml")]
    public class Views_Scrabble_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Scrabble.Models.Game>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/functions.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/css/index.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
  
    ViewData["Title"] = "Scrabble";

#line default
#line hidden
            BeginContext(73, 8, true);
            WriteLiteral("<html>\r\n");
            EndContext();
            BeginContext(81, 937, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a54ebd0c6f634c8ebdc79bc79609777b", async() => {
                BeginContext(87, 829, true);
                WriteLiteral(@"
    <link rel=""stylesheet"" href=""https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css"" integrity=""sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T"" crossorigin=""anonymous"">
    <link href=""//netdna.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"" rel=""stylesheet"">
    <script src=""//code.jquery.com/jquery-3.3.1.js"" crossorigin=""anonymous""></script>
    <script src=""https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"" integrity=""sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1"" crossorigin=""anonymous""></script>
    <script src=""https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"" integrity=""sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM"" crossorigin=""anonymous""></script>
    ");
                EndContext();
                BeginContext(916, 41, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f85589d2ca5c4162800cc94727fd4e2e", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(957, 6, true);
                WriteLiteral("\r\n    ");
                EndContext();
                BeginContext(963, 46, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "767eb860467b4c498e0c7646416374d8", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(1009, 2, true);
                WriteLiteral("\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1018, 46, true);
            WriteLiteral("\r\n\r\n<style>\r\n    .grid-item {\r\n        width: ");
            EndContext();
            BeginContext(1066, 46, false);
#line 18 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
           Write(Model.Board.IntToCSSWidth(Model.Board.Columns));

#line default
#line hidden
            EndContext();
            BeginContext(1113, 21, true);
            WriteLiteral("%;\r\n    }\r\n</style>\r\n");
            EndContext();
            BeginContext(1134, 4363, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "0341462f6a8a4cba8c2519cd12e7d045", async() => {
                BeginContext(1140, 120, true);
                WriteLiteral("\r\n    <div id=\"game\">\r\n        <div id=\"userCommands\">\r\n            <div id=\"player\">\r\n                <div id=\"rack\">\r\n");
                EndContext();
#line 26 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                     foreach (Player p in Model.Players)
                    {
                        

#line default
#line hidden
#line 28 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                         if (p.AtHand)
                        {
                            int rackIndex = 1;

#line default
#line hidden
                BeginContext(1456, 33, true);
                WriteLiteral("                            <p>\r\n");
                EndContext();
#line 32 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                                 foreach (Rack_CharTile c in p.Rack.Rack_CharTiles)
                                {

                                    

#line default
#line hidden
#line 35 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                                     for (int i = 0; i < c.Count; i++)
                                    {

#line default
#line hidden
                BeginContext(1722, 47, true);
                WriteLiteral("                                        <button");
                EndContext();
                BeginWriteAttribute("id", " id=\"", 1769, "\"", 1816, 4);
                WriteAttributeValue("", 1774, "rack_chartile_", 1774, 14, true);
#line 37 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
WriteAttributeValue("", 1788, rackIndex, 1788, 12, false);

#line default
#line hidden
                WriteAttributeValue("", 1800, "_", 1800, 1, true);
#line 37 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
WriteAttributeValue("", 1801, c.CharTileID, 1801, 15, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1817, 127, true);
                WriteLiteral(" class=\"btn btn-default btn-lg rack_chartile\">\r\n                                            <span class=\"rack_chartile_letter\">");
                EndContext();
                BeginContext(1946, 17, false);
#line 38 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                                                                           Write(c.CharTile.Letter);

#line default
#line hidden
                EndContext();
                BeginContext(1964, 93, true);
                WriteLiteral("</span>\r\n                                            <span class=\"rack_chartile_score small\">");
                EndContext();
                BeginContext(2059, 16, false);
#line 39 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                                                                                Write(c.CharTile.Score);

#line default
#line hidden
                EndContext();
                BeginContext(2076, 60, true);
                WriteLiteral("</span>\r\n                                        </button>\r\n");
                EndContext();
#line 41 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                                        rackIndex++;
                                    }

#line default
#line hidden
#line 42 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                                     
                                }

#line default
#line hidden
                BeginContext(2264, 34, true);
                WriteLiteral("                            </p>\r\n");
                EndContext();
#line 45 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                            break;
                        }

#line default
#line hidden
#line 46 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                         
                    }

#line default
#line hidden
                BeginContext(2384, 24, true);
                WriteLiteral("                </div>\r\n");
                EndContext();
                BeginContext(2604, 74, true);
                WriteLiteral("            </div>\r\n            <div id=\"controls\">\r\n                <p>\r\n");
                EndContext();
                BeginContext(2833, 370, true);
                WriteLiteral(@"                    <button id=""showAnchors"" class=""btn btn-default btn-lg""><span>Anchors</span></button>
                    <button id=""submit"" class=""btn btn-default btn-lg""><span>Submit</span></button>
                    <button id=""clearPlacements"" class=""btn btn-default btn-lg""><span>Clear</span></button>
                </p>
            </div>           
");
                EndContext();
                BeginContext(3370, 295, true);
                WriteLiteral(@"        </div>
        <div id=""gameAndOutput"">
            <div id=""output"" class=""panel panel-primary"">
                <div class=""panel-heading"">
                    <span id=""statusMessage"">Message will show here.</span>
                </div>
                <div class=""panel-body"">");
                EndContext();
                BeginContext(3667, 9, false);
#line 74 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                                    Write(Model.Log);

#line default
#line hidden
                EndContext();
                BeginContext(3677, 58, true);
                WriteLiteral("</div>\r\n            </div>\r\n            <div id=\"board\">\r\n");
                EndContext();
#line 77 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                  BoardTile[,] boardAs2DArray = Model.Board.ConvertTo2DArray(); 

#line default
#line hidden
                BeginContext(3818, 16, true);
                WriteLiteral("                ");
                EndContext();
#line 78 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                 for (int i = 0; i < boardAs2DArray.GetLength(0); i++)
                {

#line default
#line hidden
                BeginContext(3909, 24, true);
                WriteLiteral("                    <div");
                EndContext();
                BeginWriteAttribute("id", " id=\"", 3933, "\"", 3948, 2);
                WriteAttributeValue("", 3938, "row-", 3938, 4, true);
#line 80 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
WriteAttributeValue("", 3942, i+1, 3942, 6, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(3949, 21, true);
                WriteLiteral(" class=\"board_row\">\r\n");
                EndContext();
#line 81 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                         for (int j = 0; j < boardAs2DArray.GetLength(1); j++)
                        {

                            

#line default
#line hidden
#line 84 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                             if (boardAs2DArray[i, j] != null && boardAs2DArray[i, j].CharTile != null)
                            {

#line default
#line hidden
                BeginContext(4215, 36, true);
                WriteLiteral("                                <div");
                EndContext();
                BeginWriteAttribute("id", " id=\"", 4251, "\"", 4336, 4);
                WriteAttributeValue("", 4256, "tile_", 4256, 5, true);
#line 86 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
WriteAttributeValue("", 4261, boardAs2DArray[i,j].BoardLocationX, 4261, 37, false);

#line default
#line hidden
                WriteAttributeValue("", 4298, "_", 4298, 1, true);
#line 86 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
WriteAttributeValue("", 4299, boardAs2DArray[i,j].BoardLocationY, 4299, 37, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginWriteAttribute("class", " class=\"", 4337, "\"", 4403, 3);
                WriteAttributeValue("", 4345, "grid-item", 4345, 9, true);
                WriteAttributeValue(" ", 4354, "locked", 4355, 7, true);
#line 86 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
WriteAttributeValue(" ", 4361, boardAs2DArray[i,j].BoardTileType.Type, 4362, 41, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(4404, 43, true);
                WriteLiteral(">\r\n                                    <div");
                EndContext();
                BeginWriteAttribute("id", " id=\"", 4447, "\"", 4500, 2);
                WriteAttributeValue("", 4452, "board_chartile_", 4452, 15, true);
#line 87 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
WriteAttributeValue("", 4467, boardAs2DArray[i,j].CharTileID, 4467, 33, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(4501, 64, true);
                WriteLiteral(" class=\"filled\">\r\n                                        <span>");
                EndContext();
                BeginContext(4566, 36, false);
#line 88 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                                         Write(boardAs2DArray[i, j].CharTile.Letter);

#line default
#line hidden
                EndContext();
                BeginContext(4602, 84, true);
                WriteLiteral("</span>\r\n                                        <span class=\"board_chartile_score\">");
                EndContext();
                BeginContext(4687, 35, false);
#line 89 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                                                                      Write(boardAs2DArray[i, j].CharTile.Score);

#line default
#line hidden
                EndContext();
                BeginContext(4722, 127, true);
                WriteLiteral("</span>\r\n                                    </div>                                  \r\n                                </div>\r\n");
                EndContext();
#line 92 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                            }
                            else
                            {

#line default
#line hidden
                BeginContext(4945, 36, true);
                WriteLiteral("                                <div");
                EndContext();
                BeginWriteAttribute("id", " id=\"", 4981, "\"", 5066, 4);
                WriteAttributeValue("", 4986, "tile_", 4986, 5, true);
#line 95 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
WriteAttributeValue("", 4991, boardAs2DArray[i,j].BoardLocationX, 4991, 37, false);

#line default
#line hidden
                WriteAttributeValue("", 5028, "_", 5028, 1, true);
#line 95 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
WriteAttributeValue("", 5029, boardAs2DArray[i,j].BoardLocationY, 5029, 37, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginWriteAttribute("class", " class=\"", 5067, "\"", 5126, 2);
                WriteAttributeValue("", 5075, "grid-item", 5075, 9, true);
#line 95 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
WriteAttributeValue(" ", 5084, boardAs2DArray[i,j].BoardTileType.Type, 5085, 41, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(5127, 204, true);
                WriteLiteral(">\r\n                                    <span id=\"board_chartile_0\">\r\n                                        <br />\r\n                                    </span>  \r\n                                </div>\r\n");
                EndContext();
#line 100 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                            }

#line default
#line hidden
#line 100 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                             
                        }

#line default
#line hidden
                BeginContext(5389, 28, true);
                WriteLiteral("                    </div>\r\n");
                EndContext();
#line 103 "C:\Users\Simeon\Desktop\Scrabble\Scrabble\Views\Scrabble\Index.cshtml"
                }

#line default
#line hidden
                BeginContext(5436, 54, true);
                WriteLiteral("            </div>\r\n        </div>      \r\n    </div>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(5497, 11, true);
            WriteLiteral("\r\n</html>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Scrabble.Models.Game> Html { get; private set; }
    }
}
#pragma warning restore 1591
