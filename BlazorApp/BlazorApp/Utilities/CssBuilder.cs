namespace BlazorApp.Utilities
{
    public class CssBuilder
    {
        private string css;

        public static CssBuilder Default(string value) => new(value);
        public static CssBuilder Empty() => new();

        public CssBuilder() => css = String.Empty;
        public CssBuilder(string value) => css = value;

        public CssBuilder AddValue(string value)
        {
            css += value;
            return this;
        }
        public CssBuilder AddClass(string value) => AddValue(" " + value);
        public CssBuilder AddClass(string value, bool when = true) => when ? this.AddClass(value) : this;
        public CssBuilder AddClass(string value, bool? when = true) => when == true ? this.AddClass(value) : this;
        public CssBuilder AddClass(string value, Func<bool>? when = null) => this.AddClass(value, when != null && when());
        public CssBuilder AddClass(Func<string> value, bool when = true) => when ? this.AddClass(value()) : this;
        public CssBuilder AddClass(Func<string> value, Func<bool>? when = null) => this.AddClass(value, when != null && when());
        public CssBuilder AddClass(CssBuilder builder, bool when = true) => when ? this.AddClass(builder.Build()) : this;


        public string Build()
        {
            return css != null ? css.Trim() : String.Empty;
        }

        public override string ToString() => Build();
    }
}
