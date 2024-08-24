import { Entity, Vector3 } from "alt-client";
import { 
  getShapeTestResultIncludingMaterial, 
  startExpensiveSynchronousShapeTestLosProbe 
} from "natives";

export enum IntersectFlags {
    Everything = -1,
    Map = 1,
    MissionEntities = 2,
    Peds = 12,
    Objects = 16,
    Vegetation = 256,
}

export class RaycastResult {
    public DidHit: boolean
    public HitPosition: Vector3
    public SurfaceNormal: Vector3
    public MaterialHash: number
    public HitEntity: Entity | null

    constructor(handle: number) {
        const result = getShapeTestResultIncludingMaterial(handle)
        this.DidHit = result[0] === 2 && result[1]
        this.HitPosition = result[2]
        this.SurfaceNormal = result[3]
        this.MaterialHash = result[4]
        this.HitEntity = Entity.getByScriptID(result[5]) || null
    }
}

// Point to Point Raycast
export function raycast(source: Vector3, target: Vector3, options: IntersectFlags, ignoreEntity: Entity) {
    const handle = startExpensiveSynchronousShapeTestLosProbe(source.x, source.y, source.z, target.x, target.y, target.z, options, (ignoreEntity) ? ignoreEntity : 0, 7)
    return new RaycastResult(handle)
}