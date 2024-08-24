import * as alt from "alt-client";
import * as native from "natives";
import { raycast, IntersectFlags } from './raycast.js';

alt.on('keydown', (key) => {
    if(key == 69) { // E
        const player = alt.Player.local;
        const forwardVector = { x: 0, y: 1, z: 0 }; // TODO Nochmal prüfen! GroundZ benötigt?
        const pointToPointResult = raycast(player.pos, forwardVector, IntersectFlags.Map);

        if (pointToPointResult.DidHit) {
            alt.emitServer('MineralResources.Event.PickUp', pointToPointResult.MaterialHash);
        }
	}
});



// Notification für Testzwecke, statt Konsole
export function Notification(imageName, headerMsg, detailsMsg, message) {
    native.beginTextCommandThefeedPost('STRING');
    native.addTextComponentSubstringPlayerName(message);
    native.endTextCommandThefeedPostMessagetextTu(
        imageName.toUpperCase(),
        imageName.toUpperCase(),
        false,
        4,
        headerMsg,
        detailsMsg,
        1.0,
        ''
    );
    native.endTextCommandThefeedPostTicker(false, false);
}
alt.onServer('sendNotification', (message) => {
	Notification('MAIL', '', '', message);
});