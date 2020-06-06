<template>
  <div style="display:flex;flex:1">
    <el-row>
      <el-col :span="18">
        <snail-search-table
          search-api="userQueryList"
          :search-fields="searchFields"
          :fields="fields"
          :search-rules="rules"
          @row-click="selectUser"
        >

        </snail-search-table>
      </el-col>
      <el-col :span="6">
        <el-checkbox-group v-model="userRoles">
          <el-checkbox v-for="(val,index) in keyValues.roles" :key="index" style="width:100%" :label="val.name"></el-checkbox>
        </el-checkbox-group>
        <el-button @click="saveUserRoles">保存</el-button>
      </el-col>
    </el-row>
  </div>
</template>

<script>
export default {
  data() {
    return {
      tableDatas: [],
      submitApi: '',
      formData: {},
      userRoles: [],
      visible: false,
      pagination: { pageIndex: 1, pageSize: 15, total: 0 },
      loading: false,
      selectedUser: {},
      keyValues: {
        genders: [],
        roles: []
      },
      rules: {
        name: [
          { required: true, message: '必填项', trigger: 'blur' }
        ],
        account: [
          { required: true, message: '必填项', trigger: 'blur' }
        ]
      }
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
    saveUserRoles() {
      var roleKeys = this.keyValues.roles.filter(a => this.userRoles.indexOf(a.name) > -1).map(a => a.id) || []

      this.$api.setUserRoles({
        userKey: this.selectedUser.id,
        roleKeys
      }).then(res => {
        console.log('保存成功')
        this.$message.success('成功')
      })
    },
    selectUser(row, column) {
      this.selectedUser = row
      this.$api.getUserRoles({ userKey: row.id }).then(res => {
        var roleIds = res.data.roleKeys || []
        this.userRoles = this.keyValues.roles.filter(a => roleIds.indexOf(a.id) > -1).map(a => a.name) || []

        console.log(res)
      })
    },
    init() {
      this.keyValues.genders = [
        {
          key: 'male',
          value: '男'
        }, {
          key: 'female',
          value: '女'
        }
      ]
      Promise.all([
        this.$api.getAllRole()
      ]).then(res => {
        this.keyValues.roles = res[0].data
      })
    },
    selectFormatter(row, column, cellValue, index) {
      return cellValue
      // return this.$util.keyValueFormart(this.keyValues.yesnos, cellValue);
    },
    timeFormatter(row, column, cellValue, index) {
      return this.$dayjs(cellValue).format('YYYY-MM-DD')
    }
  }
}
</script>
