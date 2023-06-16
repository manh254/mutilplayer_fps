using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] TextMeshPro bulletUI;
    [SerializeField] Image healthbarImage;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float sprintSpeed;

    [SerializeField] float walkSpeed;

    [SerializeField] float jumpForce;

    [SerializeField] float smoothTime;

    
    public int itemIndex;
    int previousItemIndex= -1;
    bool IsPausing;
    bool isGrounded;
    float verticalLookRotation;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;
    [SerializeField] SingleShotGun singleShotGun;
    PhotonView PV;
    CinemachineBrain brain ;
    CinemachineVirtualCamera Vcam ;
    public GameObject pauseMenu;
    public bool IsMoving { get; set; } = false;
    const float maxHealth = 100f;
    float currentHealth = maxHealth;
    PlayerManager playerManager;
    [SerializeField] Item[] items;

    void Awake()
    {

        //virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>(true);
        IsPausing = false;
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        if (PV.IsMine)
        {
            virtualCamera.gameObject.SetActive(true);
        }
    }

    void Start()
    {
        if(PV.IsMine)
        {
            
            EquipItem(0);
        }else{
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(UI);
        }
    }

    void Update()
    {
        if(!PV.IsMine) return;

        LookAround();
        Move();
        Jump();
        if(IsPausing == false){
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        if(Input.GetKey(KeyCode.LeftAlt))
        {
          Cursor.visible = true;
        }
        
        for (int i = 0; i < items.Length; i++)
        {
            if(Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
            
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel")>0f){
            if(itemIndex<= 0){
                return;
            }
            
            EquipItem(itemIndex - 1);
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel")<0f){
            if(itemIndex>= items.Length-1){
                return;
            }
            EquipItem(itemIndex + 1);
        }
        //Debug.Log("amoo:   " + singleShotGun.currentAmmo);
        if (itemIndex == 0) {
            if (Input.GetMouseButton(0) && singleShotGun.currentAmmo > 0 && !singleShotGun.isReloading)
            {
                SingleShotGun.Instance.IsShooting = true;

                items[itemIndex].Use();
                virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain =  10f;
                

            }
            if (Input.GetMouseButtonUp(0)||  singleShotGun.currentAmmo <= 0 || singleShotGun.isReloading)
            {
                SingleShotGun.Instance.IsShooting = false;
                
                virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0f;
            }
        }

        if(itemIndex == 1){
        if(Input.GetMouseButtonDown(0) && singleShotGun.currentAmmo > 0 && !singleShotGun.isReloading)
        {
                SingleShotGun.Instance.IsShooting = true;
            items[itemIndex].Use();
            
        }
            if (Input.GetMouseButtonUp(0) || singleShotGun.currentAmmo <= 0 || singleShotGun.isReloading)
            {
                SingleShotGun.Instance.IsShooting = false;
               
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPausing)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if(transform.position.y < -10f)
        {
            Die();
        }
        
    }


    public void Pause()
    {
        Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        pauseMenu.gameObject.SetActive(true);
        IsPausing = true;
    }

    public void Resume()
    {
        pauseMenu.gameObject.SetActive(false);
         Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        IsPausing = false;
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void LeaveRoom(){
        if (PhotonNetwork.InRoom)
        {
            Destroy(RoomManager.Instance.gameObject);
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("Menu");
        }
        
        else
        {
            Debug.LogError("Not in a room, cannot leave room.");
        }
        
    }

    // public override void OnLeftRoom(){
	// 	SceneManager.LoadScene("Menu");
	// }

    void Move(){
        IsMoving = true;
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if(moveDir != Vector3.zero)
        {
            IsMoving = true;
        }
        else IsMoving = false;
        //Debug.Log("is moving??? " + IsMoving);
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift)? sprintSpeed : walkSpeed),ref smoothMoveVelocity,smoothTime);
    }

    void Jump(){
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            rb.AddForce(transform.up * jumpForce);
        }
    }
    void LookAround()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

   
    public void SetGroundState(bool _isGrounded){
        this.isGrounded = _isGrounded;
    }

    void FixedUpdate()
    {
        if(!PV.IsMine)
        {
            return;
        }
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    void EquipItem(int _index){

        if(_index == previousItemIndex){
            return;
        }
        itemIndex = _index;
        items[itemIndex].gameObject.SetActive(true);
        if(previousItemIndex != -1)
        {
            items[previousItemIndex].gameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        if(PV.IsMine)
        {
            
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(changedProps.ContainsKey("itemIndex") && !PV.IsMine && targetPlayer == PV.Owner)
        {
            Debug.Log("hehe");
            EquipItem((int)changedProps["itemIndex"]);
        }
        
    }
    //this method will run on shooter's computer
    public void TakeDamage(float damage)
    {
        PV.RPC(nameof(RPC_TakeDamage), PV.Owner, damage);
    }
    
    //runs on everyone's computer, but !PV.IsMine check makes it only run on victim's computer
    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo info)
    {
        

        Debug.Log("Took damage" + damage);
        currentHealth -= damage;
        healthbarImage.fillAmount = currentHealth / maxHealth;
        if(currentHealth <= 0){
            Die();
            PlayerManager.Find(info.Sender).GetKill();
        }
    }

    void Die()
    {
        playerManager.Die();
    }

    
}
