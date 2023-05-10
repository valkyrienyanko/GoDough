using Godot;

namespace GoDough.Runtime {
  public class GodotApi : IGodotApi {
    public PackedScene LoadScene(string fileName) {
      return (PackedScene)this.LoadResource(fileName);
    }

    public Resource LoadResource(string fileName) {
      return GD.Load(fileName);
    }
  }
}