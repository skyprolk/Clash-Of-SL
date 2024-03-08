using System;
using System.Collections.Generic;
using System.Reflection;
using CSS.Files.CSV;

namespace CSS.Files.Logic
{
    internal class Data
    {
        readonly int m_vGlobalID;

        public Data(CSVRow row, DataTable dt)
        {
            m_vCSVRow = row;
            m_vDataTable = dt;
            m_vGlobalID = GlobalID.CreateGlobalID(dt.GetTableIndex() + 1, dt.GetItemCount());
        }

        protected CSVRow m_vCSVRow;
        protected DataTable m_vDataTable;

        public static void LoadData(Data obj, Type objectType, CSVRow row)
        {
            foreach (var prop in objectType.GetProperties())
            {
                if (prop.PropertyType.IsGenericType)
                {
                    var listType = typeof(List<>);
                    var genericArgs = prop.PropertyType.GetGenericArguments();
                    var concreteType = listType.MakeGenericType(genericArgs);
                    var newList = Activator.CreateInstance(concreteType);

                    var add = concreteType.GetMethod("Add");

                    var indexerName =
                        ((DefaultMemberAttribute)
                            newList.GetType().GetCustomAttributes(typeof(DefaultMemberAttribute), true)[0]).MemberName;
                    var indexerProp = newList.GetType().GetProperty(indexerName);

                    for (var i = row.GetRowOffset(); i < row.GetRowOffset() + row.GetArraySize(prop.Name); i++)
                    {
                        var v = row.GetValue(prop.Name, i - row.GetRowOffset());
                        if (v == string.Empty && i != row.GetRowOffset())
                            v = indexerProp.GetValue(newList, new object[] { i - row.GetRowOffset() - 1 }).ToString();

                        if (v == string.Empty)
                        {
                            var o = genericArgs[0].IsValueType ? Activator.CreateInstance(genericArgs[0]) : "";
                            add.Invoke(newList, new[] { o });
                        }
                        else
                            add.Invoke(newList, new[] { Convert.ChangeType(v, genericArgs[0]) });
                    }
                    prop.SetValue(obj, newList);
                }
                else
                {
                    if (row.GetValue(prop.Name, 0) == string.Empty)
                        prop.SetValue(obj, null, null);
                    else
                        prop.SetValue(obj, Convert.ChangeType(row.GetValue(prop.Name, 0), prop.PropertyType), null);
                }
            }
        }

        public int GetDataType() => m_vDataTable.GetTableIndex();

        public int GetGlobalID() => m_vGlobalID;

        public int GetInstanceID() => GlobalID.GetInstanceID(m_vGlobalID);

        public string GetName()  => m_vCSVRow.GetName();

        public static implicit operator int(Data v)
        {
            throw new NotImplementedException();
        }
    }
}
