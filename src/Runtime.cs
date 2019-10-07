namespace RPGScript
{
    public class Runtime
    {
        public readonly Table Globals;
        public readonly API API;
        public Runtime(Table globals, API api)
        {
            Globals = globals ?? new Table();
            API = api;
        }
    }
}
