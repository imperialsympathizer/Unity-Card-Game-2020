using System;
using UnityEngine;

public class CurvedLayout : MonoBehaviour {
    // Draw a big circle
    // Put objects on the top of big circle
    // adjust rotation accordingly

    // TODO: make global variable
    private float radius = 2200;

    // TODO: make global variable
    private float padDegrees = 3;

    private float yPadding;

    GameObject mainCanvas;


    private void Awake() {
        mainCanvas = GameObject.Find("Display");
    }

    private void Start() {
        // adjust yPadding as needed based on the location of the hand object
        yPadding = mainCanvas.GetComponent<RectTransform>().sizeDelta.y * 0.08f;
        UpdateCardPositions();
    }

    /// <summary>
    /// This method is used for repositioning the cards in hand in a generic use case
    /// Additionally, it can be used to update card positions if a card in hand is being hovered over/zoomed in on
    /// </summary>
    /// <param name="zooming">Set to true if there is a card being zoomed in on</param>
    /// <param name="handIndex">Only use this parameter when zooming = true, handIndex of the card being zoomed</param>
    public void UpdateCardPositions(bool zooming = false, int handIndex = 0) {
        // When zooming a card, anything to the left of the handIndex should me moved left
        // Anything to the right of the handIndex should be moved right
        // If zooming is true, the handIndex of the card being zoomed should be passed
        int childCount = this.transform.childCount;

        // Optimal degreeDelta is 4 unless hand size is 7 or higher
        float degreeDelta = 4 - (Math.Max(0, childCount - 6) * 0.4f - Math.Max(0, childCount - 9) * 0.15f);
        // Go from left to right
        float currentDegrees = 90 + ((childCount - 1) * degreeDelta / 2);

        // Get the current positions of all cards in hand and cancel all animations attached to them
        for (int i = 0; i < childCount; i++) {
            GameObject child = this.transform.GetChild(i).gameObject;
            LeanTween.cancel(child);
            float degreeLoc = currentDegrees - degreeDelta * i;

            // Don't adjust the degrees of the card being zoomed
            if (zooming) {
                // This variable affects the distance cards move as they get further from the zoom index
                // Distance to pad is adjusted depending on how close the card is to the one being zoomed
                // The farther apart they are, the less the card will be padded
                // As child count increases, the nearest cards will be pushed further so they can still be hovered over smoothly
                // However, the effect is more pronounced on the right side of the hand compared to the left (due to how cards are stacked)
                // So the constant is also offset by handIndex at the end to mitigate this
                float difference = Math.Abs(handIndex - i) * (1.8f - childCount * 0.15f) + handIndex * 0.02f;
                
                if (i < handIndex) {
                    degreeLoc += (float)(padDegrees / Math.Sqrt(difference));
                }
                else {
                    degreeLoc -= (float)(padDegrees / Math.Sqrt(difference));
                }
            }

            // Only move if it isn't the card being zoomed (or if nothing is being zoomed)
            if ((zooming && i != handIndex) || !zooming) {
                Vector3 newPosition = new Vector3(0, 0, 0);
                newPosition.x = (float)(Math.Cos((double)(degreeLoc * Math.PI / 180)) * radius);
                newPosition.y = (float)(Math.Sin((double)(degreeLoc * Math.PI / 180)) * radius) - radius - yPadding;

                // Animate hand movement
                LeanTween.scale(child, new Vector3(1, 1, 1), 0.2f);
                LeanTween.moveLocal(child, newPosition, 0.2f);
                LeanTween.rotate(child, new Vector3(0, 0, degreeLoc - 90), 0.2f);
            }
        }
    }


    /// <summary>
    /// Very similar to the above method, but operates slightly differently as there is one fewer card in hand than the method will "think"
    /// </summary>
    /// <param name="phantomIndex">Index of where the card would be if it were in the hand</param>
    public void UpdatePositionsFromDrag(int phantomIndex) {
        // Fudging the numbers here, pretending as if the card being dragged is in the hand
        int childCount = this.transform.childCount + 1;

        float degreeDelta = 4 - (Math.Max(0, childCount - 6) * 0.4f - Math.Max(0, childCount - 9) * 0.15f);
        float currentDegrees = 90 + ((childCount - 1) * degreeDelta / 2);

        for (int i = 0; i < childCount - 1; i++) {
            GameObject child = this.transform.GetChild(i).gameObject;
            LeanTween.cancel(child);
            float degreeLoc = currentDegrees - degreeDelta * i;

            // Once i is even with phantomIndex is the first card that should be to the right
            // Decrement handIndex to make the math function correctly
            if (phantomIndex == i) {
                phantomIndex--;
            }

            float constant = 1.5f;
            float difference = Math.Abs(phantomIndex - i) * constant;
            if (i < phantomIndex) {
                degreeLoc += (float)(padDegrees / Math.Sqrt(difference));
            }
            else {
                degreeLoc -= (float)(padDegrees / Math.Sqrt(difference));
            }

            Vector3 newPosition = new Vector3(0, 0, 0);
            newPosition.x = (float)(Math.Cos((double)(degreeLoc * Math.PI / 180)) * radius);
            newPosition.y = (float)(Math.Sin((double)(degreeLoc * Math.PI / 180)) * radius) - radius - yPadding;

            // Animate hand movement
            LeanTween.scale(child, new Vector3(1, 1, 1), 0.2f);
            LeanTween.moveLocal(child, newPosition, 0.2f);
            LeanTween.rotate(child, new Vector3(0, 0, degreeLoc - 90), 0.2f);
        }
    }

    public Vector3 WhereShouldCardBe(int handIndex) {
        float yPadding = mainCanvas.GetComponent<RectTransform>().sizeDelta.y * 0.4f;

        int numChildren = this.transform.childCount;

        // Optimal degreeDelta is 4 unless hand size is 7 or higher
        float degreeDelta = 4 - (Math.Max(0, numChildren - 6) * 0.4f - Math.Max(0, numChildren - 9) * 0.15f);
        // Go from left to right
        float currentDegrees = 90 + ((numChildren - 1) * degreeDelta / 2);

        // Get the current positions of all cards in hand and cancel all animations attached to them
        float degreeLoc = currentDegrees - degreeDelta * handIndex;

        Vector3 location = new Vector3(0, 0, 0);
        location.x = (float)(Math.Cos((double)(degreeLoc * Math.PI / 180)) * radius);
        location.y = (float)(Math.Sin((double)(degreeLoc * Math.PI / 180)) * radius) - radius - yPadding;

        return location;
    }
}
