﻿#pragma strict
var roteSpeed:float = 0.0;

var animationSpeed:float =1.0;
private var animationCount:uint;
private var animationList:Array;
function Start () {
     animationCount = animation.GetClipCount();
     animationList = GetAnimationList();
}

function Update () {
  
     transform.eulerAngles.y += roteSpeed;

}
/*
function OnGUI (){
     var margin : int = 10;

     //Buttons
     var buttonSpace:int = 25;
     var rectWidth:int = 100;
     var rectHeight:int = 40;
     var max:int = 10;
     var rects:Array = new Array();
     var i:int = 0;

     for (var name : String in animationList)
     {
          var rect:Rect = Rect(15,margin + 20*i + buttonSpace*i, rectWidth,rectHeight);
          if(GUI.Button(rect,animationList[i].ToString())){
               animation.CrossFade(animationList[i],0.01);
          }
          i++;
     }
}*/

private function GetAnimationList():Array
{
     var tmpArray = new Array();
     for (var state : AnimationState in gameObject.animation)
     {
          tmpArray.Add(state.name);
     }
     return tmpArray;
}