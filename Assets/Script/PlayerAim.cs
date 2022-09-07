using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private float angle;
    public Transform _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);

        if (_player.localScale.x >= 0)
        {
            angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        }
        else if (_player.localScale.x <= 0)
        {
            angle = Mathf.Atan2(-offset.y, -offset.x) * Mathf.Rad2Deg;
        }
        transform.rotation = Quaternion.Euler(0,0,angle);
    }
}
