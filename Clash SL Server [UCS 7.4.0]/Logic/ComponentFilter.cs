namespace CSS.Logic
{
    internal class ComponentFilter : GameObjectFilter
    {
        public int Type;

        public ComponentFilter(int type)
        {
            Type = type;
        }

        public override bool IsComponentFilter() => true;

        public bool TestComponent(Component c)
        {
            GameObject go = c.GetParent();
            return TestGameObject(go);
        }

        public new bool TestGameObject(GameObject go)
        {
            bool result = false;
            Component c = go.GetComponent(Type, true);
            if (c != null)
            {
                result = base.TestGameObject(go);
            }
            return result;
        }
    }
}