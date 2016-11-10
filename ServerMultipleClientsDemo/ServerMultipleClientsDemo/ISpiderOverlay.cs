using ServerMultipleClientsDemo;

namespace SPIDER
{
    interface ISpiderOverlay
    {
        void CreateOverlay();
        void DisplayOverlay();
        Nod GetLastNode();
        int getMaximumNumberOfPeers();
        int getNrHops(Client a, Client b);
        bool IsNodeInOverlay(Client node);
        void JoinOverlay(Client existingNode);
        void LeaveOverlay(Client node);
        void PopulateOverlay();
        void UpdateAllNeihhbours(Client node);
    }
}