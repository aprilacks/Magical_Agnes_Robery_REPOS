using UnityEngine;

/* * HOW TO USE:
 * 1. Attach to a 'UI Manager' or 'Canvas' object.
 * 2. Assign your Pause Panel and Controls Panel GameObjects to the slots.
 * 3. Press 'Escape' in-game to toggle the menu.
 * 4. Ensure UI Buttons are linked to the 'Reanudar', 'OpenControls', and 'Salir' methods.
 */
public class MenuPausa : MonoBehaviour
{
    public GameObject panelPausa;
    public GameObject controlsMenu;
    private bool juegoPausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado) Reanudar();
            else Pausar();
        }
    }

    public void Pausar()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    public void Reanudar()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    public void OpenControls()
    {
        panelPausa.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void BackToPause()
    {
        controlsMenu.SetActive(false);
        panelPausa.SetActive(true);
    }

    //Modificar en el futuro para que no salga de la build sino que lleve al menu principal
    public void Salir()
    {
        Application.Quit();
    }

}

