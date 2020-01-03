<template>
  <div>
    <!-- <el-form size="mini" label-position="top">
      <el-row :gutter="20">
        <el-col :span="24">
          <el-form-item label="测试字段">
            <el-select />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="测试字段">
            <el-date-picker />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="测试字段">
            <el-input />
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="测试字段">
            <el-input />
          </el-form-item>
        </el-col>
      </el-row>
    </el-form> -->
    <snail-form :fields="fields" :data="data" />
    <button @click="getData" />
  </div>
</template>

<script>
import SnailForm from '@/components/Form/SnailForm'

export default {
  components: {
    SnailForm
  },
  data() {
    return {
      data: {
        str: 'this is str',
        date: '2019-12-12',
        select: 'yes'
      },
      fields: [
        {
          fieldName: 'str',
          fieldType: 'input',
          span: 12,
          label: '字段类型'
        },
        {
          fieldName: 'date',
          fieldType: 'date',
          span: 12,
          label: '时间'
        },
        {
          fieldName: 'select',
          fieldType: 'select',
          span: 8,
          label: '时间'
        }
      ]
    }
  },
  methods: {
    getData() {

    }
  }
}
</script>
