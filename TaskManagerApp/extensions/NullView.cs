

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

// https://gist.github.com/pauldotknopf/b424e9b8b03d31d67f3cce59f09ab17f?permalink_comment_id=3635387
// "Its doesn't work on dot net core 3.1. NullView.Instance can't
// be accessed here. Since in 3.1, NullView.Instance is no longer
// public. It is now an internal class."

// https://github.com/nopSolutions/nopCommerce/blob/develop/src/Presentation/Nop.Web.Framework/Controllers/NullView.cs
// "Create a NullView file like" the following class file.

namespace TaskManagerApp.extensions

{
    public partial class NullView : IView
    {
        public static readonly NullView Instance = new();

        public string Path => string.Empty;

        public Task RenderAsync(ViewContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            return Task.CompletedTask;
        }
    }
}