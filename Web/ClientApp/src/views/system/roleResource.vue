<template>
  <div style="display:flex;flex:1">
    <el-row style="width:100%">
      <el-col :span="8" style="height:600px">
        <el-input placeholder="搜索用户"></el-input>
        <snail-table
          ref="user"
          :rows="users"
          :fields="userFields"
          @row-click="selectUser"
        ></snail-table>
      </el-col>
      <el-col :span="8" style="height:600px">
        <el-button @click="saveUserRole">设置人员角色</el-button>
        <snail-table
          ref="role"
          :multiSelect='true'
          :rows="roles"
          :fields="roleFields"
          @selection-change="selectRoles"
        ></snail-table>
      </el-col>
      <el-col :span="8" style="height:600;overflow-y: scroll">
        <el-button @click="saveRoleResource">设置角色权限</el-button>
        <el-tree
          ref="tree"
          :data="resources"
          show-checkbox
          default-expand-all
          node-key="id"
          highlight-current
          :props="treeProps"
        >
        </el-tree>
      </el-col>
    </el-row>
  </div>
</template>

<script>
export default {
  data() {
    return {
      resources: [], // 所有资源，树结构
      roleResources: [], // 记录角色的资源id
      userRoles: [], // 记录人员的角色
      roles: [], // 所有角色列表
      users: [], // 所有用户列表
      treeProps: {
        children: 'children',
        label: 'name'
      },
      currentUserRow: {},
      currentRoleRows:[]
    }
  },
  computed: {
    roleFields() {
      return [
        {
          name: 'name',
          label: '角色'
        }
      ]
    },
    userFields() {
      return [
        {
          name: 'name',
          label: '姓名'
        },
         {
          name: 'account',
          label: '账号'
        },
      ]
    },
    searchFields() {
      return [
        {
          name: 'keyWord',
          label: '关键字',
          type: 'string'
        }
      ]
    }
  },
  created() {
    this.init()
  },
  methods: {
    saveUserRole(){
      debugger
      var roleKeys = (this.currentRoleRows || []).map(v=>v.id);
      this.$api.setUserRoles({
        userKey: this.currentUserRow.id,
        roleKeys: roleKeys
      }).then(res => {
        this.$message.success('保存成功')
        this.selectUser(this.currentUserRow)
      })
    },
    saveRoleResource() {
      if(this.currentRoleRows.length==1){
        var resourceKeys = this.$refs.tree.getCheckedKeys()
        this.$api.setRoleResources({
          roleKey: this.currentRoleRows[0].id,
          resourceKeys: resourceKeys
        }).then(res => {
          this.$message.success('保存成功')
          this.selectRole(this.currentRoleRows[0])
        })
      }else{
        this.$$message.warn("一次只能对一个角色进行资源授权")
      }
      
    },
    selectRoles(section){
      this.currentRoleRows=section;
      console.log('currentRoleRows')
      console.log(this.currentRoleRows)
       if(section.length=1){
        this.$api.getRoleResources({ roleKey: section[0].id }).then(res => {
        this.roleResources = res.data.resourceKeys || []
        this.$refs.tree.setCheckedKeys(this.roleResources)
      })
      }
    },
    selectUser(row){
      this.currentUserRow = row
      this.$api.getUserRoles({ userKey: row.id }).then(res => {
        this.userRoles = res.data.roleKeys || []
        this.setRoleSelection(this.userRoles);
      })
    },
    setRoleSelection(roleKeys){
      if(roleKeys){
        this.$refs.role.table.clearSelection();
        var matchRoles=this.roles.filter(v=>roleKeys.findIndex(v.id)>-1);
        console.log('matchRoles')
        console.log(matchRoles);
        matchRoles.forEach(role => {
          this.$refs.role.table.toggleRowSelection(role,true)
        });
      }
    },
    getAllUser(){
      this.$api.getAllUserInfo().then(res => {
              this.users=res.data
      })
    },
    getAllRole(){
      this.$api.getAllRole().then(res => {
              this.roles = res.data
      })
    },
    getAllResource(){
       this.$api.resourceQueryListTree().then(res => {
        this.resources = res.data
      })
    },
    init() {
        this.getAllUser();
        this.getAllRole();
        this.getAllResource();
    }
  }
}
</script>
