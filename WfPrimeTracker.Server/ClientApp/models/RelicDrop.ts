import { Relic } from './Relic'
import { DropChance } from './DropChance.enum';

export interface RelicDrop {
    dropChance: DropChance;
    relic: Relic;
}
