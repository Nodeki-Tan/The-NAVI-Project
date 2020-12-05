# The NAVI Project

Repo for the ongoing NAVI game project, no copyright violation intended, this is a cooperative effort by the SEL community
guided by Nodeki-tan to make a fan game about the NAVI and the Wired.
 




Help section!!!, more entries may get added here
______________________________________________________


Csproj corruption problems / compilation errors:

Before trying anything here read closely the error and check is not a missing reference of an object, if that fails
the .csproj may be corrupted or includes arent right

If by chance the .csproj has lost the includes here is an updated list, an example of this error could be:
	"assetManager doesnt inherit a Node"

Such error is caused by the .csproj losing the include of that file, in such case manually fix it or copy and paste this:

    <Compile Include="Scripts\CSharp\Managers\AssetManager.cs" />
    <Compile Include="Scripts\CSharp\Managers\ClientMessageManager.cs" />
    <Compile Include="Scripts\CSharp\Managers\ServerMessageManager.cs" />
    <Compile Include="Scripts\CSharp\Managers\ModuleManager.cs" />
    <Compile Include="Scripts\CSharp\Managers\NetworkManager.cs" />
    <Compile Include="Scripts\CSharp\Managers\ProgramManager.cs" />
    <Compile Include="Scripts\CSharp\Managers\WorldManager.cs" />
    <Compile Include="Scripts\CSharp\Utils\TimeUtils.cs" />
    <Compile Include="Scripts\CSharp\Utils\UtilsBox.cs" />
    <Compile Include="Scripts\CSharp\Entities\Network\2D\KinematicPuppetEntity.cs" />
    <Compile Include="Scripts\CSharp\Entities\Network\2D\NetworkEntity.cs" />
    <Compile Include="Scripts\CSharp\Entities\Network\2D\PuppetEntity.cs" />
    <Compile Include="Scripts\CSharp\Entities\Network\2D\PuppetPlayer.cs" />
    <Compile Include="Scripts\CSharp\Menus\MainMenu.cs" />
    <Compile Include="Scripts\CSharp\Menus\MainMenuController.cs" />
    <Compile Include="Scripts\CSharp\Entities\Ladder.cs" />
    <Compile Include="Scripts\CSharp\Data\Player\PlayerCredentials.cs" />
    <Compile Include="Scripts\CSharp\Entities\Local\2D\Player\PlayerEntity.cs" />
    <Compile Include="Scripts\CSharp\Data\NAVI\NAVI.cs" />
    <Compile Include="Scripts\CSharp\Data\NAVI\NAVIProgram.cs" />
    <Compile Include="Scripts\CSharp\Data\NAVI\Module.cs" />
    <Compile Include="Scripts\CSharp\Core\GameCore.cs" />
    <Compile Include="Scripts\CSharp\Core\MapCore\MapCore.cs" />

Thats the hole compile include list in case something happens, as more .cs are added the list will get longer and may be
moved to other archive




	SEL community and Nodeki-tan 12/2020, L E T S   A L L   L O V E   L A I N
