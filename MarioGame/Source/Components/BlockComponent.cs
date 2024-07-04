using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Components;

public class BlockComponent : BaseComponent
{
    public bool IsDestrucible { get; set; }
    public BlockType BlockType { get; set; }

    public BlockComponent(bool isDestrucible, BlockType blockType)
    {
        IsDestrucible = isDestrucible;
        BlockType = blockType;
    }
}
