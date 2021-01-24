using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Core.Common.MobOrganisation
{
    public static class MobsOrganisationSystem
    {
        private static List<MobsGroup> Groups;

        private static List<MobInformation> MobsInformation;

        private static Transform TransformPlayer;

        public static Guid createdMob(Transform mobTransform)
        {
            var mobInfo = new MobInformation().Init(mobTransform);
            MobsInformation.Add(mobInfo);

            return mobInfo.Id;
        }

        public static void deletedMob(Guid id)
        {
            MobsInformation.Remove(MobsInformation.FirstOrDefault(e => e.Id == id));
        }

        public static void Init(Transform transformPlayer, int countMobsInGroup)
        {
            TransformPlayer = transformPlayer;
            
            Groups = new List<MobsGroup>();
            for (var i = 0; i < countMobsInGroup; i ++)
            {
                Groups.Add(new MobsGroup());
            }
        }

        public static void selectMainMob(Guid groupId)
        {
            var group = Groups.FirstOrDefault(e => e.Id == groupId);

            if (group != null)
            {
                var main = group.MobsInformationGroup
                    .OrderByDescending(e => Vector2.Distance(e.TransformMob.position, TransformPlayer.position))
                    .FirstOrDefault();

                group.IdMobMain = main.Id;
            }
        }

        public static bool IsMainGroup(MobInformation mob)
        {
            foreach(var group in Groups)
            {
                if (group.IdMobMain == mob.Id)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class MobInformation
    {
        public Transform TransformMob { get; private set; }

        public Guid Id = Guid.NewGuid();

        public MobInformation Init(Transform transformMob)
        {
            TransformMob = transformMob;

            return this;
        }
    }

    public class MobsGroup
    {
        public Guid Id = Guid.NewGuid();

        public Guid IdMobMain { get; set; }


        public List<MobInformation> MobsInformationGroup;
        
        public MobInformation GetMainMob()
        {
            return MobsInformationGroup.FirstOrDefault(e => e.Id == IdMobMain);
        }

    }
}
