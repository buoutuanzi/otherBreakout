using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool _isActive;
    private Vector3 _ballPosition;
    private Vector2 _ballInitialForce;
    private Rigidbody2D _rb;

    public GameObject player;
    public GameObject gameControl;

    private Vector2 _reflectForce;
    private Vector2 _horizontalPreventionForce;

    private enum Speed {Slower, Regular, Faster};//��ʾ�������״̬���Ա�֮�����ֺͱ��
    private Speed _currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _ballInitialForce = new Vector2(200f, 400f); 
        _isActive = false;
        _ballPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _horizontalPreventionForce = new Vector2(0f, -50f);
        _reflectForce = new Vector2(0f, 0f); //�ȴ�������һ����Vector������ײʱ�ټ���x��y���ٶȾ���
    }

    public void Fire()
    {
        if (!_isActive)
        {
            ForceReset(); //������ʱ��һ���С�����ȥ���õ�ͬʱ�Ե����ߣ����������������������Ա�֤��ʼ���ͷ�������ͳһ
            _rb.AddForce(_ballInitialForce);
            _isActive = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive && player)
        {
            _ballPosition.x = player.transform.position.x;
            transform.position = _ballPosition;
        }

        if (_isActive && transform.position.y < -5.5)
        {
            _isActive = false;
            _ballPosition.x = player.transform.position.x;
            _ballPosition.y = -4.258f;
            transform.position = _ballPosition;
            _rb.velocity = Vector2.zero; //����ʱ��velocity�����Է�����
            gameControl.SendMessage("TakeLife");
            player.SendMessage("LengthReset");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && _isActive)
        {
            _rb.velocity = Vector2.zero;//�����Է�����
            float xDistanceToMiddle = (transform.position.x - player.transform.position.x) / 1.6f;
            //�����Ǹ�����ײʱ�����ĵľ��룬�ٳ������ߵ��м�ľ���İٷֱ���������������x����
            //ƽ̨һ��ľ�������1.25�������õ�1.6����Ϊ�е�ʱ�������ΪС����ײ�����ߣ�С��İ뾶����Ϊ�����ľ����һ���֣�
            //������������Ͼ�������������ȡһ�����ֵ1.6����ֹx����������������Ԥ�ڣ��������ƶ�����
            _reflectForce.x = xDistanceToMiddle * 250;
            _rb.AddForce(_reflectForce);
        } else if (collision.gameObject.tag == "Block")
        {
            collision.gameObject.SendMessage("GetHit");
        } else if (collision.gameObject.tag == "Wall")
        {//�ڼ�������£�С���������ǽ֮��ˮƽ����������Ϸ�޷��������У���������⵱��ײ��ǽ��ʱ�Ƿ�y��velocity��0
            if (_isActive && Mathf.Abs(_rb.velocity.y) <= 1)
            {
                Debug.Log("Ball is moving horizontally");
                _rb.AddForce(_horizontalPreventionForce); //��y�����ٶ�Ϊ0���ֶ����һ��������ֹ��Ϸ��ס
            }
        }
    }

    private void OnEnable()
    {
        BallFast.fast += PowerUpFasterReceived;
        BallSlow.slow += PowerUpSlowerReceived;
    }

    private void OnDisable()
    {
        BallFast.fast -= PowerUpFasterReceived;
        BallSlow.slow -= PowerUpSlowerReceived;
    }

    private void PowerUpSlowerReceived()
    {   //�������С����й����гԵ�������ߣ�С��Ҳ��Ҫ���̱任�ٶȣ���������Ȱ�С��ǰ���ٶȰ������úõı�������
        //����ÿ��ײ�����ƽ̨�������㣬����ʹ�������Բ��õ��Ļ�Ӱ��֮����ٶ�
        Vector2 newVelocity = new Vector2(_rb.velocity.x, _rb.velocity.y) * VelocityMultiplierSlower();
        Debug.Log("ԭ��y�ٶ���" + _rb.velocity.y);
        _rb.velocity = Vector2.zero; //��յ�ǰ�ٶȷ�ֹ��ʱ����Ч
        _rb.velocity = newVelocity;
        Debug.Log("����y�ٶ���" + _rb.velocity.y);
        switch (_currentSpeed)
        {
            case Speed.Slower:
                break;
            case Speed.Regular:
                _currentSpeed = Speed.Slower;
                break;
            case Speed.Faster:
                _currentSpeed = Speed.Regular;
                break;
        }
        ForceRefresh();
    }

    private void PowerUpFasterReceived()
    {
        Vector2 newVelocity = new Vector2(_rb.velocity.x, _rb.velocity.y) * VelocityMultiplierFaster();
        Debug.Log("ԭ��y�ٶ���" + _rb.velocity.y);
        _rb.velocity = Vector2.zero;
        _rb.velocity = newVelocity;
        Debug.Log("����y�ٶ���" + _rb.velocity.y);
        switch (_currentSpeed)
        {
            case Speed.Slower:
                _currentSpeed = Speed.Regular;
                break;
            case Speed.Regular:
                _currentSpeed = Speed.Faster;
                break;
            case Speed.Faster:
                break;
        }
        ForceRefresh();
    }

    private void ForceRefresh()
    {//��Ȼ˵����������Ҫ���ٶȣ���������С���������ƶ������Ըı��ٶȸ��������Ǹı���ƽ̨�Ӵ�ʱʩ�ӵ����Ĵ�С
        //���ٶ�ֻ��������С�����ڷ��еĹ����л��õ�
        Debug.Log("Speed change, ball speed status is" + _currentSpeed);
        switch (_currentSpeed)
        {
            case Speed.Slower:
                _reflectForce.y = 250;
                break;
            case Speed.Regular:
                _reflectForce.y = 400;
                break;
            case Speed.Faster:
                _reflectForce.y = 550;
                break;
        }
    }

    private void ForceReset()
    {
        _currentSpeed = Speed.Regular;
        ForceRefresh();
    }

    private float VelocityMultiplierFaster() //�������ú�velocity��Ҫ�˵ı�����ƥ�����ı仯
    {
        switch (_currentSpeed)
        {
            case Speed.Slower:
                return 1.6f;
            case Speed.Regular:
                return 1.375f;
            case Speed.Faster:
                return 1f;
        }
        return float.NaN;
    }

    private float VelocityMultiplierSlower()
    {
        switch (_currentSpeed)
        {
            case Speed.Slower:
                return 1f;
            case Speed.Regular:
                return 0.625f;
            case Speed.Faster:
                return 0.728f;
        }
        return float.NaN;
    }
}
