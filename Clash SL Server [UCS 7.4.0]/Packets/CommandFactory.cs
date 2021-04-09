using System;
using System.Collections.Generic;
using System.IO;
using CSS.Core;
using CSS.Helpers;
using CSS.Packets.Commands;
using CSS.Packets.Commands.Client;

namespace CSS.Packets
{
    internal class CommandFactory
    {
        public static Dictionary<int, Type> Commands;

        public CommandFactory()
        {
            Commands = new Dictionary<int, Type>
            {
                {14, typeof(PlayerWarStatusCommand)},
                {20, typeof(RemainingBuildingsLayoutCommand)},
                {500, typeof(BuyBuildingCommand)},
                {501, typeof(MoveBuildingCommand)},
                {502, typeof(UpgradeBuildingCommand)},
                {503, typeof(SellBuildingCommand)},
                {504, typeof(SpeedUpConstructionCommand)},
                {505, typeof(CancelConstructionCommand)},
                {506, typeof(CollectResourcesCommand)},
                {507, typeof(ClearObstacleCommand)},
                {508, typeof(TrainUnitCommand)},
                {509, typeof(CancelUnitProductionCommand)},
                {510, typeof(BuyTrapCommand)},
                {511, typeof(RequestAllianceUnitsCommand)},
                {512, typeof(BuyDecoCommand)},
                {513, typeof(SpeedUpTrainingCommand)},
                {514, typeof(SpeedUpClearingCommand)},
                {515, typeof(CancelUpgradeUnitCommand)},
                {516, typeof(UpgradeUnitCommand)},
                {517, typeof(SpeedUpUpgradeUnitCommand)},
                {518, typeof(BuyResourcesCommand)},
                {519, typeof(MissionProgressCommand)},
                {520, typeof(UnlockBuildingCommand)},
                {521, typeof(FreeWorkerCommand)},
                {522, typeof(BuyShieldCommand)},
                {523, typeof(ClaimAchievementRewardCommand)},
                {524, typeof(ToggleAttackModeCommand)},
                {525, typeof(LoadTurretCommand)},
                {526, typeof(BoostBuildingCommand)},
                {527, typeof(UpgradeHeroCommand)},
                {528, typeof(SpeedUpHeroUpgradeCommand)},
                {529, typeof(ToggleHeroSleepCommand)},
                {530, typeof(SpeedUpHeroHealthCommand)},
                {531, typeof(CancelHeroUpgradeCommand)},
                {532, typeof(NewShopItemsSeenCommand)},
                {533, typeof(MoveMultipleBuildingsCommand)},
                {534, typeof(CancelShieldCommand)},
                {537, typeof(SendAllianceMailCommand)},
                {538, typeof(MyLeagueCommand)},
                {539, typeof(NewsSeenCommand)},
                {540, typeof(RequestAllianceUnitsCommand)},
                {541, typeof(SpeedUpRequestUnitsCommand)},
                {543, typeof(KickAllianceMemberCommand)},
                {544, typeof(GetVillageLayoutsCommand)},
                {546, typeof(EditVillageLayoutCommand)},
                {549, typeof(UpgradeMultipleBuildingsCommand)},
                {550, typeof(RemoveUnitsCommand)},
                {552, typeof(SaveVillageLayoutCommand)},
                {553, typeof(ClientServerTickCommand)},
                {554, typeof(RotateDefenseCommand)},
                {558, typeof(AddQuicKTrainingTroopCommand)},
                {559, typeof(TrainQuickUnitsCommand)},
                {560, typeof(StartClanWarCommand)},
                {567, typeof(SetActiveVillageLayoutCommand)},
                {568, typeof(CopyVillageLayoutCommand)},
                {570, typeof(TogglePlayerWarStateCommand)},
                {571, typeof(FilterChatCommand)},
                {572, typeof(ToggleHeroAttackModeCommand)},
                {574, typeof(ChallangeCommand)},
                {577, typeof(MoveBuildingsCommand)},
                {584, typeof(BoostBarracksCommand)},
                {586, typeof(RenameQuickTrainCommand)},
                {590, typeof(EventsSeenCommand)},
                {600, typeof(PlaceAttackerCommand)},
                {601, typeof(PlaceAllianceTroopsCommand)},
                {603, typeof(EndOfBattleCommand)},
                {604, typeof(CastSpellCommand)},
                {605, typeof(PlaceHeroCommand)},
                {700, typeof(SearchOpponentCommand)}
            };

            //Commands.Add(0, typeof(UnknownCommand));
            //Commands.Add(1, typeof(JoinAlliance));
            //Commands.Add(2, typeof(LeaveAllianceCommand));
            //Commands.Add(3, typeof(ChangeAvatarCommand));
            //Commands.Add(5, typeof());
            //Commands.Add(551, typeof(ContinueBarrackBoostCommand));
            //Commands.Add(563, typeof(CollectClanResourcesCommand));
            //Commands.Add(573, typeof(RemoveShieldToAttackCommand));
        }
    }
}
