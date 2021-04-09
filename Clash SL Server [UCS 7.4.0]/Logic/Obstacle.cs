using Newtonsoft.Json.Linq;
using System;
using CSS.Core;
using CSS.Helpers;
using CSS.Files.Logic;

namespace CSS.Logic
{
	internal class Obstacle : GameObject
	{
		private readonly Level m_vLevel;

		private Timer m_vTimer;

		public Obstacle(Data data, Level l) : base(data, l)
		{
			m_vLevel = l;
		}

        public override int ClassId => 3;

		public void CancelClearing()
		{
			m_vLevel.WorkerManager.DeallocateWorker(this);
			m_vTimer = null;
			var od = GetObstacleData();
			var rd = od.GetClearingResource();
			var cost = od.ClearCost;
			Avatar.Avatar.CommodityCountChangeHelper(0, rd, cost);
		}

		public void ClearingFinished()
		{
			//m_vLevel.GameObjectManager.GetObstacleManager().IncreaseObstacleClearCount();
			m_vLevel.WorkerManager.DeallocateWorker(this);
			m_vTimer = null;
			var constructionTime = GetObstacleData().ClearTimeSeconds;
            var exp = (int)Math.Sqrt(constructionTime);

            Avatar.Avatar.AddExperience(exp);

			var rd = CSVManager.DataTables.GetResourceByName(GetObstacleData().LootResource);

			Avatar.Avatar.CommodityCountChangeHelper(0, rd, GetObstacleData().LootCount);

			Avatar.GameObjectManager.RemoveGameObject(this);
		}

        public ObstacleData GetObstacleData() => (ObstacleData)GetData();

        public int GetRemainingClearingTime() => m_vTimer.GetRemainingSeconds(m_vLevel.Avatar.LastTickSaved);

        public bool IsClearingOnGoing() => m_vTimer != null;

        public void SpeedUpClearing()
		{
			var remainingSeconds = 0;
			if (IsClearingOnGoing())
			{
				remainingSeconds = m_vTimer.GetRemainingSeconds(m_vLevel.Avatar.LastTickSaved);
			}
			var cost = GamePlayUtil.GetSpeedUpCost(remainingSeconds);
			var ca = Avatar.Avatar;
			if (ca.HasEnoughDiamonds(cost))
			{
				ca.UseDiamonds(cost);
				ClearingFinished();
			}
		}

		public void StartClearing()
		{
			var constructionTime = GetObstacleData().ClearTimeSeconds;
			if (constructionTime < 1)
			{
				ClearingFinished();
			}
			else
			{
				m_vTimer = new Timer();
				m_vTimer.StartTimer(constructionTime, m_vLevel.Avatar.LastTickSaved);
				m_vLevel.WorkerManager.AllocateWorker(this);
			}
		}

		public override void Tick()
		{
			if (IsClearingOnGoing())
			{
				if (m_vTimer.GetRemainingSeconds(m_vLevel.Avatar.LastTickSaved) <= 0)
					ClearingFinished();
			}
		}

		public JObject ToJson()
		{
			var jsonObject = new JObject();
			jsonObject.Add("data", GetObstacleData().GetGlobalID());
			if (IsClearingOnGoing())
				jsonObject.Add("const_t", m_vTimer.GetRemainingSeconds(m_vLevel.Avatar.LastTickSaved));
			jsonObject.Add("x", X);
			jsonObject.Add("y", Y);
			return jsonObject;
		} 
	}
}
