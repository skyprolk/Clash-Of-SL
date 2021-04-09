using System.Collections.Generic;
using CSS.Helpers.Binary;
using CSS.Logic;

namespace CSS.Packets.Commands.Client
{
	internal class TrainQuickUnitsCommand : Command
	{
	    public TrainQuickUnitsCommand(Reader reader, Device client, int id) : base(reader, client, id)
	    {
	        
	    }

	    internal override void Decode()
		{
		    this.DataSlotID = this.Reader.ReadInt32(); 
			this.Tick = this.Reader.ReadInt32(); 
		}

	    public int DataSlotID;
	    public int Tick;

		internal override void Process()
		{
			var player = this.Device.Player.Avatar;

			if (DataSlotID == 1)
			{
				foreach (DataSlot i in player.QuickTrain1)
				{
                    List<DataSlot> _PlayerUnits = player.GetUnits();
                    DataSlot _DataSlot = _PlayerUnits.Find(t => t.Data.GetGlobalID() == i.Data.GetGlobalID());
                    if (_DataSlot != null)
                    {
                        _DataSlot.Value = _DataSlot.Value + i.Value;
                    }
                    else
                    {
                        DataSlot ds = new DataSlot(i.Data, i.Value);
                        _PlayerUnits.Add(ds);
                    }
                }
            }
			else if (DataSlotID == 2)
			{
				foreach (DataSlot i in player.QuickTrain2)
				{
                    List<DataSlot> _PlayerUnits = player.GetUnits();
                    DataSlot _DataSlot = _PlayerUnits.Find(t => t.Data.GetGlobalID() == i.Data.GetGlobalID());
                    if (_DataSlot != null)
                    {
                        _DataSlot.Value = _DataSlot.Value + i.Value;
                    }
                    else
                    {
                        DataSlot ds = new DataSlot(i.Data, i.Value);
                        _PlayerUnits.Add(ds);
                    }
                }
			}
			else if (DataSlotID == 3)
			{
				foreach (DataSlot i in player.QuickTrain3)
				{
                    List<DataSlot> _PlayerUnits = player.GetUnits();
                    DataSlot _DataSlot = _PlayerUnits.Find(t => t.Data.GetGlobalID() == i.Data.GetGlobalID());
                    if (_DataSlot != null)
                    {
                        _DataSlot.Value = _DataSlot.Value + i.Value;
                    }
                    else
                    {
                        DataSlot ds = new DataSlot(i.Data, i.Value);
                        _PlayerUnits.Add(ds);
                    }
                }
			}			
		}
	}
}
