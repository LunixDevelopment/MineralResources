import { Entity } from "alt-client";
import { getShapeTestResultIncludingMaterial, startExpensiveSynchronousShapeTestLosProbe } from "natives";
export var IntersectFlags;
(function (IntersectFlags) {
    IntersectFlags[IntersectFlags["Everything"] = -1] = "Everything";
    IntersectFlags[IntersectFlags["Map"] = 1] = "Map";
    IntersectFlags[IntersectFlags["MissionEntities"] = 2] = "MissionEntities";
    IntersectFlags[IntersectFlags["Peds"] = 12] = "Peds";
    IntersectFlags[IntersectFlags["Objects"] = 16] = "Objects";
    IntersectFlags[IntersectFlags["Vegetation"] = 256] = "Vegetation";
})(IntersectFlags || (IntersectFlags = {}));
export class RaycastResult {
    DidHit;
    HitPosition;
    SurfaceNormal;
    MaterialHash;
    HitEntity;
    constructor(handle) {
        const result = getShapeTestResultIncludingMaterial(handle);
        this.DidHit = result[0] === 2 && result[1];
        this.HitPosition = result[2];
        this.SurfaceNormal = result[3];
        this.MaterialHash = result[4];
        this.HitEntity = Entity.getByScriptID(result[5]) || null;
    }
}
export function raycast(source, target, options, ignoreEntity) {
    const handle = startExpensiveSynchronousShapeTestLosProbe(source.x, source.y, source.z, target.x, target.y, target.z, options, (ignoreEntity) ? ignoreEntity : 0, 7);
    return new RaycastResult(handle);
}
