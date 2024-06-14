using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Linq;
using Protocols.Common;

public partial class NetworkAPI 
{
    public static async UniTask<EServerStatus> GetServerStatus(CancellationToken cancellationToken)
    {
        var serverVersions = await UnityHttp.Get<VersionData[]>($"{ServerSettings.commonUrl}/version_list.json", cancellationToken: cancellationToken);
        var versionData = serverVersions.FirstOrDefault();
        ServerSettings.Set(versionData);
        return versionData.status;
    }

    public static async UniTask<MaintenanceData[]> GetMainteranceData(CancellationToken cancellationToken)
    {
        var data = await UnityHttp.GetData($"{ServerSettings.commonUrl}/maintenance_notice.json", cancellationToken: cancellationToken);
        MaintenanceData[] maintenanceArr = data.GetResult<MaintenanceData[]>();
        return maintenanceArr;
        //return System.Array.FindAll<MaintenanceData>(maintenanceArr, x =>
        //{
        //    if (x.os == ETargetOS.All)
        //        return true;

        //    var osCode = GameAPI.GetOSCode();
        //    if (x.os == ETargetOS.Android
        //        && osCode == OSCode.Android)
        //        return true;

        //    if (x.os == ETargetOS.iOS
        //        && osCode == OSCode.iOS)
        //        return true;

        //    return false;
        //});
    }
}
