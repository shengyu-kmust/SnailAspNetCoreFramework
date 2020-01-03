<template>
  <div>
    <el-form style="margin:10px">
      <el-row :gutter="20">
        <template v-for="item in fields">
          <el-col :span="item.span">
            <el-form-item
              v-bind="item"
            >
              <el-input v-if="item.fieldType==='input'" v-model="data[item.fieldName]" />
              <el-date-picker v-if="item.fieldType==='datetime'" v-model="data[item.fieldName]" style="width:100%" type="datetime" v-bind="item" />
              <el-date-picker v-if="item.fieldType==='date'" v-model="data[item.fieldName]" style="width:100%" type="date" v-bind="item" />
              <el-select v-if="item.fieldType==='select'" v-model="data[item.fieldName]" style="width:100%" v-bind="item" />
            </el-form-item>
          </el-col>

        </template>
      </el-row>
    </el-form>
  </div>
</template>

<script>
export default {
  name: 'SnailForm',
  props: {
    data: Object,
    fields: Array
  },
  created() {
    console.log(this.data)
    console.log(this.fields)
  },
  methods: {

  }
}
</script>
