# -*- coding: utf-8 -*-
# Generated by Django 1.9 on 2015-12-07 21:58
from __future__ import unicode_literals

from django.db import migrations, models


class Migration(migrations.Migration):

    initial = True

    dependencies = [
    ]

    operations = [
        migrations.CreateModel(
            name='Medium',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('label', models.CharField(max_length=64)),
                ('device_type', models.CharField(choices=[('CDR', 'CD-ROM'), ('DVD', 'DVD'), ('HDD', 'Hard Disk Drive')], max_length=3)),
                ('serial_number', models.CharField(max_length=64)),
                ('base_path', models.CharField(max_length=200)),
                ('question_text', models.CharField(max_length=200)),
                ('created_date', models.DateTimeField()),
                ('updated_date', models.DateTimeField()),
            ],
        ),
    ]