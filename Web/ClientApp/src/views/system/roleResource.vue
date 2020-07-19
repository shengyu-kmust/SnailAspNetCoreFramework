<template>
  <div style="display:flex;flex:1">
    <el-row style="width:100%">
      <el-col :span="12" style="height:100%">
        <snail-table
          ref="table"
          :rows="roles"
          :fields="roleFields"
          @row-click="selectRole"
        ></snail-table>
      </el-col>
      <el-col :span="12" style="height:100%">
        <el-button @click="save">保存</el-button>
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
      roles: [], // 所有角色列表
      treeProps: {
        children: 'children',
        label: 'name'
      },
      currentRow: {}
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
    fields() {
      return [
        {
          name: 'name',
          label: '姓名'
        },
        {
          name: 'account',
          label: '账号'
        },
        {
          name: 'phone',
          label: '电话'

        },
        {
          name: 'email',
          label: '邮箱'
        },
        {
          name: 'gender',
          label: '性别',
          formatter: this.selectFormatter
        }
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
    save() {
      var resourceKeys = this.$refs.tree.getCheckedKeys()
      this.$api.setRoleResources({
        roleKey: this.currentRow.id,
        resourceKeys: resourceKeys
      }).then(res => {
        this.$message.success('保存成功')
        this.selectRole(this.currentRow)
      })
    },
    selectRole(row) {
      this.currentRow = row
      this.$api.getRoleResources({ roleKey: row.id }).then(res => {
        this.roleResources = res.data.resourceKeys || []
        this.$refs.tree.setCheckedKeys(this.roleResources)
      })
    },
    init() {
      this.$api.resourceQueryListTree().then(res => {
        this.resources = res.data
      })
      this.$api.getAllRole().then(res => {
        this.roles = res.data
      })
    }
  }
}
</script>
