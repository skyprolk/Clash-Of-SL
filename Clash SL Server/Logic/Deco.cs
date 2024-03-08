using Newtonsoft.Json.Linq;
using CSS.Files.Logic;

namespace CSS.Logic
{
    internal class Deco : GameObject
    {
        Level m_vLevel;

        public Deco(Data data, Level l) : base(data, l)
        {
            m_vLevel = l;
        }

        public override int ClassId => 6;

        public DecoData GetDecoData() => (DecoData)GetData();

        public new void Load(JObject jsonObject)
        {
            base.Load(jsonObject);
        }

        public new JObject Save(JObject jsonObject)
        {
            base.Save(jsonObject);
            return jsonObject;
        }
    }
}