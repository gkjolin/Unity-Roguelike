using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Behaviour that results in items being dropped upon an entity's death.
    /// </summary>
    public sealed class OnDeathDropItems : MonoBehaviour, IOnDeath
    {
        public ItemClass ItemClass { get { return itemClass; } }
        [SerializeField] ItemClass itemClass;

        public int NumitemsToDrop { get { return numItemsToDrop; } }
        [SerializeField] int numItemsToDrop = 1;

        IItemFactory itemFactory;
        Ground ground;

        public void Initialize(IItemFactory itemFactory, Ground ground)
        {
            this.itemFactory = itemFactory;
            this.ground = ground;
        }

        public void Invoke()
        {
            for (int i = 0; i < numItemsToDrop; i++)
            {
                ItemTemplate chosenTemplate = itemClass.FetchItem();
                if (chosenTemplate != null)
                {
                    Item item = itemFactory.Build(chosenTemplate);
                    ground.TryPlaceItemOnGround(item, transform.position);
                }
            }
        }

        void OnValidate()
        {
            numItemsToDrop = Mathf.Max(0, numItemsToDrop);
        }
    }
}