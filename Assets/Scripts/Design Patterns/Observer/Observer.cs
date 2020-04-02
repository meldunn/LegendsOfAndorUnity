public interface Observer
{
    void UpdateData(string Category);       // Not called Update to avoid overriding default Update method
}
