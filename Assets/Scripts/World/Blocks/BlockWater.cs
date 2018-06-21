using UnityEngine;
using System.Collections;

public class BlockWater : Block {
    //How many blocks is this block away from the origin of the water
    public int DistanceFromMain = 0;
    //on how many seconds should it try to place new blocks around it
    float tickInterval = .2f;
    float timer;

    //we ovveride the Tick from the upper class
    public override void Tick(float deltaTime) {
        timer += deltaTime;

        if (timer > tickInterval) {
            timer = 0;

            if (DistanceFromMain < 10) //if there's still room to place
            {
                bool continueSideways = true; //we take it so that we can place blocks around it

                //However, if there's no block below us,
                Block bottomBlock = world.GetBlock(blockX, blockY - 1, blockZ);

                if (bottomBlock != null) {
                    if (bottomBlock.GetType() == typeof(BlockAir)) {
                        //then we can't place other blocks sideways
                        continueSideways = false;
                        //we set a new block of water below us
                        world.SetBlock(blockX, blockY - 1, blockZ, new BlockWater());

                        //the above function doesn't return the block
                        //so we have to take it manually
                        BlockWater newBlock = world.GetBlock(blockX, blockY - 1, blockZ) as BlockWater;

                        //If there is a new block to get, which means we are not on the edge of our grid
                        if (newBlock != null) {
                            //then take the new block we just added
                            newBlock.SetWorld(world, blockX, blockY - 1, blockZ);
                            //and add it into the register for Update list
                            newBlock.RegisterForUpdate();
                        }
                    } else  //We also cannot place any other water blocks sidewas if there's water below us
                        if (bottomBlock.GetType() == typeof(BlockWater)) {
                        continueSideways = false;
                    }
                }

                //If we checked what is below us, and we can place sideways
                if (continueSideways == true) {
                    //Then check for every direction, instead of up
                    Block forwardBlock = world.GetBlock(blockX, blockY, blockZ + 1);

                    //and basically do the same as above
                    if (forwardBlock != null) {
                        if (forwardBlock.GetType() == typeof(BlockAir)) {
                            world.SetBlock(blockX, blockY, blockZ + 1, new BlockWater());
                            BlockWater newBlock = world.GetBlock(blockX, blockY, blockZ + 1) as BlockWater;

                            if (newBlock != null) {
                                newBlock.SetWorld(world, blockX, blockY, blockZ + 1);
                                newBlock.RegisterForUpdate();
                                newBlock.DistanceFromMain = DistanceFromMain + 1; //But add to the distance from main
                                //we only take into account the distance from the main water block when we are moving
                                //sideways because otherwise our water would have stopped mid air
                            }
                        }
                    }

                    //Do the same for all the directions, except up
                    Block backBlock = world.GetBlock(blockX, blockY, blockZ - 1);

                    if (backBlock != null) {
                        if (backBlock.GetType() == typeof(BlockAir)) {
                            world.SetBlock(blockX, blockY, blockZ - 1, new BlockWater());
                            BlockWater newBlock = world.GetBlock(blockX, blockY, blockZ - 1) as BlockWater;

                            if (newBlock != null) {
                                newBlock.SetWorld(world, blockX, blockY, blockZ - 1);
                                newBlock.RegisterForUpdate();
                                newBlock.DistanceFromMain = DistanceFromMain + 1;
                            }
                        }
                    }

                    Block leftBlock = world.GetBlock(blockX - 1, blockY, blockZ);

                    if (leftBlock != null) {
                        if (leftBlock.GetType() == typeof(BlockAir)) {
                            world.SetBlock(blockX - 1, blockY, blockZ, new BlockWater());
                            BlockWater newBlock = world.GetBlock(blockX - 1, blockY, blockZ) as BlockWater;

                            if (newBlock != null) {
                                newBlock.SetWorld(world, blockX - 1, blockY, blockZ);
                                newBlock.RegisterForUpdate();
                                newBlock.DistanceFromMain = DistanceFromMain + 1;
                            }
                        }
                    }

                    Block rightBlock = world.GetBlock(blockX + 1, blockY, blockZ);

                    if (rightBlock != null) {
                        if (rightBlock.GetType() == typeof(BlockAir)) {
                            world.SetBlock(blockX + 1, blockY, blockZ, new BlockWater());
                            BlockWater newBlock = world.GetBlock(blockX + 1, blockY, blockZ) as BlockWater;

                            if (newBlock != null) {
                                newBlock.SetWorld(world, blockX + 1, blockY, blockZ);
                                newBlock.RegisterForUpdate();
                                newBlock.DistanceFromMain = DistanceFromMain + 1;
                            }
                        }
                    }
                }
            }
        }
    }

    public override Tile TexturePosition(Direction direction) {
        Tile tile = new Tile();
        switch (direction) {
            case Direction.Up:
                tile.x = 3;
                tile.y = 3;
                break;

            case Direction.Down:
                tile.x = 3;
                tile.y = 3;
                break;

            default:
                tile.x = 3;
                tile.y = 3;
                break;
        }

        return tile;

    }

}
