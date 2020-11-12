/**
 * 组件的命名规则
 * 功能约定
 * 
 */
options=
 {
     fields:[// 定义界面上所有的字段类型的数据，如table的列字段，搜索的字段，form表单的字段
         {
            name:'',//字段名
            type:'',//字段类型，支持：string,int,datetime,date,select,multiSelect,time,check,radio
            label:'',//字段描述文字
            span:12,//栅格数，为int，全部栅格为24
            keyValues:[
                {
                    key:'key',//为后端存储的值
                    value:'value',//为值所显示的内容
                    extraInfo:''//其它补充信息
                }
            ],//下拉的keyValues，用于talbe和form的下拉数据的显示   
            width:'',// 宽度，如table的列宽，格式如10px，50%
            formatter:function(){},//为格式化函数
            defaultValue:'',// 默认值，用于form表单的默认
            required:true,// 是否必填，用于form表单的验证
         }
     ],
     searchFieldNames:[''],// 用于搜索的字段
     tableFieldNames:[''],// table的列字段
     searchApiName:'',//默认数据搜索api名
     addApiName:'',//增加数据api名
     editApiName:'',//修改数据api名
     findApiName:'',//查看单表数据api名
     removeApiName:''//增加数据api名

 }