using UnityEngine;
using System.Collections;

public class TriggerCalibrator : MonoBehaviour {

    public bool isLeft;
    private SteamVR_TrackedController device;
    private GameController gameController;

    void Start ( ) {
        device = GetComponent<SteamVR_TrackedController> ( );
        device.TriggerClicked += Trigger;
        device.Gripped += Gripped;
        GameObject gameControllerObject = GameObject.FindWithTag ( "GameController" );
        if ( gameControllerObject != null ) {
            gameController = gameControllerObject.GetComponent<GameController> ( );
        }
        if ( gameController == null ) {
            Debug.Log ( "Cannot find 'GameController' script" );
        }
    }

    void Trigger ( object sender, ClickedEventArgs e ) {
        if ( isLeft && !gameController.LeftCalibrated ) {
            gameController.CalibrateLeft ( );
        } else if ( !isLeft && !gameController.RightCalibrated ) {
            gameController.CalibrateRighft ( );
        }
    }
    void Gripped ( object sender, ClickedEventArgs e ) {
        if (  !gameController.HeadCalibrated ) {
            gameController.CalibrateHead ( );
        }
    }
}
