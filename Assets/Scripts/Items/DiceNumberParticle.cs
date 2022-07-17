using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceNumberParticle : MonoBehaviour
{
    public Vector2 moveSpeedMinMax = new Vector2(10, 20);

    public float time = 2f;

    public AnimationCurve opacityCurve = AnimationCurve.EaseInOut(1, 0, 1, 0);

    public TMPro.TMP_Text text;

    public void Show(Vector3 position, int value)
    {
        DiceNumberParticle dnp = Instantiate(this.gameObject, position, Quaternion.identity).GetComponent<DiceNumberParticle>();
        dnp.text.text = "" + value;
        dnp.StartCoroutine(dnp.Animate());
    }

    private IEnumerator Animate()
    {
        Vector3 direction = Random.insideUnitCircle.normalized;
        direction.y = Mathf.Abs(direction.y);

        float speed = Random.Range(moveSpeedMinMax.x, moveSpeedMinMax.y);

        float t = 0;
        while (t < time)
        {
            Color c = text.color;
            c.a = opacityCurve.Evaluate(t / time);
            text.color = c;

            transform.position += Time.deltaTime * speed * opacityCurve.Evaluate(t / time) * direction;

            t += Time.deltaTime;
            yield return 0;
        }

        Destroy(this.gameObject);
    }
}
